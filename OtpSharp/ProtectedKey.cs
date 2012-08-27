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
        readonly int keyLength;
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
            if (!(key != null))
                throw new ArgumentNullException("A secret key must be provided");
            if (!(key.Length > 0))
                throw new ArgumentException("The key must not be empty");

            this.keyLength = (isProtected && keyLength > 0) ? keyLength : key.Length;
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
            var plainKey = new byte[this.keyLength];

            lock (this.stateSync)
            {
                try
                {
                    if (this.isProtected)
                    {
                        ProtectedMemory.Unprotect(this.protectedKeyData, MemoryProtectionScope.SameProcess);
                        this.isProtected = false;

                    }
                    Array.Copy(this.protectedKeyData, plainKey, this.keyLength);
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
