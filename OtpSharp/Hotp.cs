
namespace OtpSharp
{
    /// <summary>
    /// Calculate HMAC-One-Time-Passwords (HOTP) from a secret key
    /// </summary>
    public class Hotp : Otp
    {
        /// <summary>
        /// Create an HOTP instance
        /// </summary>
        /// <param name="secretKey">The secret key to use in HOTP calculations</param>
        public Hotp(byte[] secretKey)
            : base(secretKey)
        {
        }

        /// <summary>
        /// Create an HOTP instance
        /// </summary>
        /// <param name="secretKey">The secret key to use in HOTP calculations</param>
        public Hotp(Key secretKey)
            : base(secretKey)
        {
        }

        /// <summary>
        /// Takes a counter and produces an HOTP value
        /// </summary>
        /// <param name="counter">the counter to be incremented each time this method is called</param>
        /// <returns>Hotp</returns>
        public string ComputeHotp(long counter)
        {
            return this.Compute(counter);
        }

        /// <remarks>
        /// This method mainly exists for unit tests.
        /// The RFC defines a decimal value in the test table that is an
        /// intermediate step to a final HOTP value
        /// </remarks>
        internal long ComputeHotpDecimal(long counter)
        {
            var hashData = KeyUtilities.GetBigEndianBytes(counter);
            return this.CalculateOtp(hashData, OtpHashMode.Sha1);
        }

        /// <summary>
        /// Takes a counter and runs it through the HOTP algorithm.
        /// </summary>
        /// <param name="counter">Counter or step</param>
        /// <returns>HOTP calculated code</returns>
        protected override string Compute(long counter)
        {
            var rawValue = ComputeHotpDecimal(counter);
            return Otp.Digits(rawValue, 6); // all of the HOTP values are six digits long
        }

        /// <summary>
        /// Used in generating URLs.  HOTP
        /// </summary>
        protected override string OtpType { get { return "hotp"; } }
    }
}