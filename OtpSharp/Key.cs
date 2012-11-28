using System;

namespace OtpSharp
{
    /// <summary>
    /// Class used to represent a key
    /// </summary>
    public abstract class Key
    {
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
                throw new ArgumentNullException("useKey");

            var plainKey = GetCopyOfKey();

            try
            {
                useKey(plainKey);
            }
            finally
            {
                // wipe the key from memory by writing random stuff out to it as best as we can
                KeyUtilities.Destroy(plainKey);
            }
        }

        /// <summary>
        /// Gets a copy of the plaintext key
        /// </summary>
        /// <returns>Plaintext Key</returns>
        protected abstract byte[] GetCopyOfKey();
    }
}
