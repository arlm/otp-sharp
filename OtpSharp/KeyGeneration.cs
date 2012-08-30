
namespace OtpSharp
{
    /// <summary>
    /// Helpers to work with keys
    /// </summary>
    public class KeyGeneration
    {
        /// <summary>
        /// Generate key of the specified length
        /// </summary>
        /// <param name="length">Key length</param>
        /// <returns>The generated key</returns>
        public static byte[] GenerateKey(int length)
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
        public static byte[] GenerateKey(OtpHashMode mode = OtpHashMode.Sha1)
        {
            return GenerateKey(LengthForMode(mode));
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