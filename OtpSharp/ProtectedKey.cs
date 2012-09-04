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
        /// <remarks>
        /// This must use the SameProcess protection scope or it won't work
        /// </remarks>
        /// <param name="preProtectedKey">Pre-protected key data</param>
        /// <param name="keyLength">The length of the plaintext key (protected memory may need to be padded)</param>
        /// <param name="scope">The memory protection scope that was used to protect the memory</param>
        /// <returns>A protected key instance from the provided key</returns>
        public static ProtectedKey CreateProtectedKeyFromPreprotectedMemory(byte[] preProtectedKey, int keyLength, MemoryProtectionScope scope)
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

            this.keyLength = keyLength;
            this.isProtected = true;
            this.protectedKeyData = preProtectedKey;
            this.scope = scope;
        }

        /// <summary>
        /// Allows a delegate to use the key then tries to overwrite it from memory.
        /// Warning! Do what you need to with the key within the scope of the delegate as it will overwrite the reference when it exists.
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

            try
            {
                useKey(plainKey);
            }
            finally
            {
                // wipe the key from memory by writing random stuff out to it
                new Random().NextBytes(plainKey);
            }
        }
    }
}
