using System;
using System.Security.Cryptography;

namespace OtpSharp
{
    /// <summary>
    /// Represents a protected key
    /// </summary>
    public class ProtectedKey : Key
    {
        readonly object stateSync = new object();
        readonly byte[] protectedKeyData;
        readonly int keyLength;
        bool isProtected;
        readonly MemoryProtectionScope scope;

        #region static initializers

        /// <summary>
        /// Static method to create a protected key from a plaintext key.  It will wipe the reference that was passed in once the protected key instance is initialized
        /// </summary>
        /// <param name="plaintextKey">The key</param>
        /// <returns>A protected key instance from the provided key</returns>
        public static ProtectedKey CreateProtectedKeyAndDestroyPlaintextKey(byte[] plaintextKey)
        {
            var key = new ProtectedKey(plaintextKey);
            // the protected key creates a copy of the key and pads it as needed for in memory protection.
            // Thus the reference that was passed in isn't needed.  Overwrite it with random garbage.
            new Random().NextBytes(plaintextKey);
            return key;
        }

        /// <summary>
        /// Creates an instance of the protected key from a byte array that has already been protected using the ProtectedMemory.Protect method call.
        /// </summary>
        /// <param name="preProtectedKey">Pre-protected key data</param>
        /// <param name="keyLength">The length of the plaintext key (protected memory may need to be padded)</param>
        /// <param name="scope">The memory protection scope that was used to protect the memory</param>
        /// <returns>A protected key instance from the provided key</returns>
        public static ProtectedKey CreateProtectedKeyFromPreProtectedMemory(byte[] preProtectedKey, int keyLength, MemoryProtectionScope scope)
        {
            return new ProtectedKey(preProtectedKey, keyLength, scope);
        }

        #endregion

        /// <summary>
        /// Creates an instance of a protected key.
        /// </summary>
        /// <param name="key">Plaintext key data</param>
        public ProtectedKey(byte[] key)
        {
            if (!(key != null))
                throw new ArgumentNullException("A secret key must be provided");
            if (!(key.Length > 0))
                throw new ArgumentException("The key must not be empty");

            this.keyLength = key.Length;
            int paddedKeyLength = (int)Math.Ceiling((decimal)key.Length / (decimal)16) * 16;
            this.protectedKeyData = new byte[paddedKeyLength];
            Array.Copy(key, this.protectedKeyData, key.Length);

            this.scope = MemoryProtectionScope.SameProcess;
            ProtectedMemory.Protect(this.protectedKeyData, this.scope);

            this.isProtected = true;
        }

        private ProtectedKey(byte[] preProtectedKey, int keyLength, MemoryProtectionScope scope)
        {
            if (!(preProtectedKey != null))
                throw new ArgumentNullException("A secret key must be provided");
            if (!(preProtectedKey.Length > 0))
                throw new ArgumentException("The key must not be empty");
            if (!(keyLength > 0))
                throw new ArgumentException("The key must not be empty");

            this.keyLength = keyLength;
            this.isProtected = true;
            this.protectedKeyData = preProtectedKey;
            this.scope = scope;
        }

        /// <summary>
        /// Gets a copy of the plaintext key
        /// </summary>
        /// <returns>Plaintext Key</returns>
        protected override byte[] GetCopyOfKey()
        {
            var plainKey = new byte[this.keyLength];
            lock (this.stateSync)
            {
                try
                {
                    if (this.isProtected)
                    {
                        ProtectedMemory.Unprotect(this.protectedKeyData, this.scope);
                        this.isProtected = false;

                    }
                    Array.Copy(this.protectedKeyData, plainKey, this.keyLength);
                }
                finally
                {
                    if (!this.isProtected)
                    {
                        ProtectedMemory.Protect(this.protectedKeyData, this.scope);
                        this.isProtected = true;
                    }
                }
            }
            return plainKey;
        }
    }
}
