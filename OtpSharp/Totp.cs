using System;
using System.Globalization;

namespace OtpSharp
{
    /// <summary>
    /// Calculate Timed-One-Time-Passwords (TOTP) from a secret key
    /// </summary>
    /// <remarks>
    /// The specifications for this are found in RFC 6238
    /// http://tools.ietf.org/html/rfc6238
    /// </remarks>
    public class Totp : Otp
    {
        /// <summary>
        /// The number of ticks as Measured at Midnight Jan 1st 1970;
        /// </summary>
        const long unixEpochTicks = 621355968000000000L;
        /// <summary>
        /// A divisor for converting ticks to seconds
        /// </summary>
        const long ticksToSeconds = 10000000L;

        private readonly int step;
        private readonly OtpHashMode hashMode;
        private readonly int totpSize;
        private readonly TimeCorrection correctedTime;

        /// <summary>
        /// Create a TOTP instance
        /// </summary>
        /// <param name="secretKey">The secret key to use in TOTP calculations</param>
        /// <param name="step">The time window step amount to use in calculating time windows.  The default is 30 as recommended in the RFC</param>
        /// <param name="mode">The hash mode to use</param>
        /// <param name="totpSize">The number of digits that the returning TOTP should have.  The default is 6.</param>
        /// <param name="timeCorrection">If required, a time correction can be specified to compensate of an out of sync local clock</param>
        public Totp(byte[] secretKey, int step = 30, OtpHashMode mode = OtpHashMode.Sha1, int totpSize = 6, TimeCorrection timeCorrection = null)
            : base(secretKey)
        {
            VerifyParameters(step, totpSize);

            this.step = step;
            this.hashMode = mode;
            this.totpSize = totpSize;
            this.correctedTime = timeCorrection ?? TimeCorrection.UncorrectedInstance;
        }

        /// <summary>
        /// Create a TOTP instance
        /// </summary>
        /// <param name="secretKey">The secret key to use in TOTP calculations</param>
        /// <param name="step">The time window step amount to use in calculating time windows.  The default is 30 as recommended in the RFC</param>
        /// <param name="mode">The hash mode to use</param>
        /// <param name="totpSize">The number of digits that the returning TOTP should have.  The default is 6.</param>
        /// <param name="timeCorrection">If required, a time correction can be specified to compensate of an out of sync local clock</param>
        public Totp(Key secretKey, int step = 30, OtpHashMode mode = OtpHashMode.Sha1, int totpSize = 6, TimeCorrection timeCorrection = null)
            : base(secretKey)
        {
            VerifyParameters(step, totpSize);

            this.step = step;
            this.hashMode = mode;
            this.totpSize = totpSize;
            this.correctedTime = timeCorrection ?? TimeCorrection.UncorrectedInstance;
        }

        private static void VerifyParameters(int step, int totpSize)
        {
            if (!(step > 0))
                throw new ArgumentOutOfRangeException("The step must be a non zero positive integer");
            if (!(totpSize > 0))
                throw new ArgumentOutOfRangeException("The totp size must be a non zero positive integer");
            if (!(totpSize <= 10))
                throw new ArgumentOutOfRangeException("The totp size must be no greater than 10");
        }

        /// <summary>
        /// Takes a timestamp and applies correction (if provided) and then computes a TOTP value
        /// </summary>
        /// <param name="timestamp">The timestamp to use for the TOTP calculation</param>
        /// <returns>a TOTP value</returns>
        public int ComputeTotp(DateTime timestamp)
        {
            return ComputeTotpFromSpecificTime(this.correctedTime.GetCorrectedTime(timestamp));
        }

        /// <summary>
        /// Takes a timestamp and computes a TOTP value for corrected UTC now
        /// </summary>
        /// <remarks>
        /// It will be corrected against a corrected UTC time using the provided time correction.  If none was provided then simply the current UTC will be used.
        /// </remarks>
        /// <returns>a TOTP value</returns>
        public int ComputeTotp()
        {
            return this.ComputeTotpFromSpecificTime(this.correctedTime.CorrectedUtcNow);
        }

        private int ComputeTotpFromSpecificTime(DateTime timestamp)
        {
            var window = CalculateTimeStepFromTimestamp(timestamp);
            return (int)this.Compute(window);
        }

        /// <summary>
        /// Verify a value that has been provided with the calculated value.
        /// </summary>
        /// <remarks>
        /// It will be corrected against a corrected UTC time using the provided time correction.  If none was provided then simply the current UTC will be used.
        /// </remarks>
        /// <param name="totp">the trial TOTP value</param>
        /// <param name="timeStepMatched">
        /// This is an output parameter that gives that time step that was used to find a match.
        /// This is useful in cases where a TOTP value should only be used once.  This value is a unique identifier of the
        /// time step (not the value) that can be used to prevent the same step from being used multiple times
        /// </param>
        /// <param name="window">The window of steps to verify</param>
        /// <returns>True if there is a match.</returns>
        public bool VerifyTotp(int totp, out long timeStepMatched, VerificationWindow window = null)
        {
            return this.VerifyTotpForSpecificTime(this.correctedTime.CorrectedUtcNow, totp, window, out timeStepMatched);
        }

        /// <summary>
        /// Verify a value that has been provided with the calculated value
        /// </summary>
        /// <param name="timestamp">The timestamp to use</param>
        /// <param name="totp">the trial TOTP value</param>
        /// <param name="timeStepMatched">
        /// This is an output parameter that gives that time step that was used to find a match.
        /// This is usefule in cases where a TOTP value should only be used once.  This value is a unique identifier of the
        /// time step (not the value) that can be used to prevent the same step from being used multiple times
        /// </param>
        /// <param name="window">The window of steps to verify</param>
        /// <returns>True if there is a match.</returns>
        public bool VerifyTotp(DateTime timestamp, int totp, out long timeStepMatched, VerificationWindow window = null)
        {
            return this.VerifyTotpForSpecificTime(this.correctedTime.GetCorrectedTime(timestamp), totp, window, out timeStepMatched);
        }

        private bool VerifyTotpForSpecificTime(DateTime timestamp, int totp, VerificationWindow window, out long timeStepMatched)
        {
            var initialStep = CalculateTimeStepFromTimestamp(timestamp);
            return this.Verify(initialStep, totp, out timeStepMatched, window);
        }

        /// <summary>
        /// Takes a timestamp and calculates a time step
        /// </summary>
        private long CalculateTimeStepFromTimestamp(DateTime timestamp)
        {
            var unixTimestamp = (timestamp.Ticks - unixEpochTicks) / ticksToSeconds;
            var window = unixTimestamp / (long)this.step;
            return window;
        }

        /// <summary>
        /// Remaining seconds in current window based on UtcNow
        /// </summary>
        /// <remarks>
        /// It will be corrected against a corrected UTC time using the provided time correction.  If none was provided then simply the current UTC will be used.
        /// </remarks>
        /// <returns>Number of remaining seconds</returns>
        public int RemainingSeconds()
        {
            return RemainingSecondsForSpecificTime(this.correctedTime.CorrectedUtcNow);
        }

        /// <summary>
        /// Remaining seconds in current window
        /// </summary>
        /// <param name="timestamp">The timestamp</param>
        /// <returns>Number of remaining seconds</returns>
        public int RemainingSeconds(DateTime timestamp)
        {
            return RemainingSecondsForSpecificTime(this.correctedTime.GetCorrectedTime(timestamp));
        }

        private int RemainingSecondsForSpecificTime(DateTime timestamp)
        {
            return this.step - (int)(((timestamp.Ticks - unixEpochTicks) / ticksToSeconds) % this.step);
        }

        /// <summary>
        /// Takes a time step and computes a TOTP code
        /// </summary>
        /// <param name="counter">time step</param>
        /// <returns>TOTP calculated code</returns>
        protected override long Compute(long counter)
        {
            var data = GetBigEndianBytes(counter);
            var otp = this.CalculateOtp(data, this.hashMode);
            return Digits(otp, this.totpSize);
        }

        /// <summary>
        /// Used in generating URLs.  TOTP
        /// </summary>
        protected override string OtpType { get { return "totp"; } }

        // NO_WEB is useful in cases where only a client profile is available
#if NO_WEB
#else
        /// <summary>
        /// Gets a URL that conforms to the de-facto standard
        /// created and used by Google
        /// </summary>
        public string GetKeyUrl(string user)
        {
            if (string.IsNullOrEmpty(user))
                throw new ArgumentNullException("user");

            var url = this.GetBaseKeyUrl(user);

            if (this.hashMode != OtpHashMode.Sha1)
                url += string.Format(CultureInfo.InvariantCulture.NumberFormat, "&algorithm={0}", this.hashMode);

            if (this.step != 30)
                url += string.Format(CultureInfo.InvariantCulture.NumberFormat, "&period={0}", this.step);

            if (this.totpSize != 6)
            {
                if (this.totpSize == 8)
                    url += "&digits=8";
                else
                    throw new ArgumentException("The URL format doesn't allow for a digit size other than 8 or 6");
            }

            return url;
        }
#endif
    }
}