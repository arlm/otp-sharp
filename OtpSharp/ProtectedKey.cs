using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace OtpSharp
{
    /// <summary>
    /// Represents a protected key
    /// </summary>
    public class ProtectedKey
    {
        readonly object stateSync = new object();
        readonly byte[] protectedKeyData;
        readonly int length;
        bool isProtected;

        /// <summary>
        /// Creates an instance of a protected key.
        /// </summary>
        /// <param name="key">Plaintext key data</param>
        /// <param name="wipeKeyReference"></param>
        /// <param name="isProtected">True if the key data is already protected</param>
        public ProtectedKey(byte[] key, bool wipeKeyReference = true, bool isProtected = false)
        {
            this.length = key.Length;
            int paddedKeyLength = (int)Math.Ceiling((decimal)key.Length / (decimal)16) * 16;
            this.protectedKeyData = new byte[paddedKeyLength];
            Array.Copy(key, this.protectedKeyData, key.Length);

            if (!isProtected)
                ProtectedMemory.Protect(this.protectedKeyData, MemoryProtectionScope.SameProcess);

            this.isProtected = true;

            if (wipeKeyReference)
                new Random().NextBytes(key);
        }

        /// <summary>
        /// Uses the protected key to compute an HMAC Hash
        /// </summary>
        /// <param name="input">The data to use as the HMAC input</param>
        /// <param name="mode">The Hash algorithm to use</param>
        /// <returns>The hashed data</returns>
        public byte[] ComputeHmacHash(byte[] input, OtpHashMode mode)
        {
            byte[] hashedValue = null;

            this.UsePlainKey(key =>
            {
                using (HMAC hmac = this.CreateHmacHash(mode))
                {
                    hmac.Key = key;
                    hashedValue = hmac.ComputeHash(input);
                }
            });

            return hashedValue;
        }

        /// <summary>
        /// Create an HMAC object for the specified algorithm
        /// </summary>
        private HMAC CreateHmacHash(OtpHashMode otpHashMode)
        {
            HMAC hmacAlgorithm = null;
            switch (otpHashMode)
            {
                case OtpHashMode.Sha256:
                    hmacAlgorithm = new HMACSHA256();
                    break;
                case OtpHashMode.Sha512:
                    hmacAlgorithm = new HMACSHA512();
                    break;
                default:
                case OtpHashMode.Sha1:
                    hmacAlgorithm = new HMACSHA1();
                    break;
            }

            return hmacAlgorithm;
        }

        /// <summary>
        /// Sets uses the key then wipes it from memory
        /// </summary>
        private void UsePlainKey(Action<byte[]> useKey)
        {
            var plainKey = new byte[this.length];

            lock (this.stateSync)
            {
                try
                {
                    if (this.isProtected)
                    {
                        ProtectedMemory.Unprotect(this.protectedKeyData, MemoryProtectionScope.SameProcess);
                        this.isProtected = false;

                    }
                    Array.Copy(this.protectedKeyData, plainKey, this.length);
                }
                finally
                {
                    if (!this.isProtected)
                    {
                        ProtectedMemory.Protect(this.protectedKeyData, MemoryProtectionScope.SameProcess);
                        this.isProtected = true;
                    }
                }
            }

            useKey(plainKey);

            // wipe the key from memory by writing random stuff out to it
            new Random().NextBytes(plainKey);
        }
    }
}
