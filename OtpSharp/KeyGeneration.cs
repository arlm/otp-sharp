
using System;
namespace OtpSharp
{
    /// <summary>
    /// Helpers to work with keys
    /// </summary>
    public class KeyGeneration
    {
        /// <summary>
        /// Generates a random key in accordance with the RFC recommened length for each algorithm
        /// </summary>
        /// <param name="length">Key length</param>
        /// <returns>The generated key</returns>
        [Obsolete("Please use KeyGeneration.GenerateRandomKey instead")]
        public static byte[] GenerateKey(int length)
        {
            return GenerateRandomKey(length);
        }

        /// <summary>
        /// Generates a random key in accordance with the RFC recommened length for each algorithm
        /// </summary>
        /// <param name="length">Key length</param>
        /// <returns>The generated key</returns>
        public static byte[] GenerateRandomKey(int length)
        {
            byte[] key = new byte[length];
            var rnd = System.Security.Cryptography.RandomNumberGenerator.Create();
            rnd.GetBytes(key);
            return key;
        }

        /// <summary>
        /// Generates a key in accordance with the RFC recommened length for each algorithm
        /// </summary>
        /// <param name="mode">HashMode</param>
        /// <returns>Key</returns>
        [Obsolete("Please use KeyGeneration.GenerateRandomKey instead")]
        public static byte[] GenerateKey(OtpHashMode mode = OtpHashMode.Sha1)
        {
            return GenerateRandomKey(mode);
        }

        /// <summary>
        /// Generates a random key in accordance with the RFC recommened length for each algorithm
        /// </summary>
        /// <param name="mode">HashMode</param>
        /// <returns>Key</returns>
        public static byte[] GenerateRandomKey(OtpHashMode mode = OtpHashMode.Sha1)
        {
            return GenerateRandomKey(LengthForMode(mode));
        }

        private static int LengthForMode(OtpHashMode mode)
        {
            switch(mode)
            {
                case OtpHashMode.Sha256:
                    return 32;
                case OtpHashMode.Sha512:
                    return 64;
                default: //case OtpHashMode.Sha1:
                    return 20;
            }
        }
    }
}