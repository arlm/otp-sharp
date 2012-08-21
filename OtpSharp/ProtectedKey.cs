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
        /// <param name="keyLength">Specifies the original key lenght if the is protected flag is set</param>
        public ProtectedKey(byte[] key, bool wipeKeyReference = true, bool isProtected = false, int keyLength = 0)
        {
            this.length = (isProtected && keyLength > 0) ? keyLength : key.Length;
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
        /// Allows a delegate to use the key then tries to overwrite it from memory
        /// </summary>
        /// <remarks>
        /// This isn't foolproof as the delegate could create another copy of the key and in some cases even must.
        /// The goal here is simply to limit the exposre of the plain key in memory as much as possible
        /// </remarks>
        /// <param name="useKey">Delegate the uses the plaintext key</param>
        /// <exception cref="ArgumentNullException">thrown if no delegate is provided</exception>
        public void UsePlainKey(Action<byte[]> useKey)
        {
            if (useKey == null)
                throw new ArgumentNullException("Must provide a delegate that uses the key");
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
