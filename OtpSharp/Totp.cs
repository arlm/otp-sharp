﻿using System;
using System.Linq;
using System.Security.Cryptography;
using System.Diagnostics.Contracts;

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

        private readonly byte[] secretKey;
        private readonly int step;
        private readonly OtpHashMode hashMode;
        private readonly int totpSize;

        /// <summary>
        /// Create a TOTP instance
        /// </summary>
        /// <param name="secretKey">The secret key to use in TOTP calculations</param>
        /// <param name="step">The time window step amount to use in calculating time windows.  The default is 30 as recommended in the RFC</param>
        /// <param name="mode">The hash mode to use</param>
        /// <param name="totpSize">The number of digits that the returning TOTP should have.  The default is 6.</param>
        public Totp(byte[] secretKey, int step = 30, OtpHashMode mode = OtpHashMode.Sha1, int totpSize = 6)
        {
            if (!(step > 0))
                throw new ArgumentOutOfRangeException("The step must be a non zero positive integer");
            if (!(totpSize > 0))
                throw new ArgumentOutOfRangeException("The totp size must be a non zero positive integer");
            if (!(totpSize <= 10))
                throw new ArgumentOutOfRangeException("The totp size must be no greater than 10");
            if (!(secretKey != null))
                throw new ArgumentNullException("A secret key must be provided");
            if (!(secretKey.Length > 0))
                throw new ArgumentException("The key must not be empty");

            this.secretKey = secretKey;
            this.step = step;
            this.hashMode = mode;
            this.totpSize = totpSize;
        }

        /// <summary>
        /// Takes a timestamp and computes a TOTP value
        /// </summary>
        /// <param name="timestamp">The timestamp to use for the TOTP calculation</param>
        /// <returns>a TOTP value</returns>
        public int ComputeTotp(DateTime timestamp)
        {
            var window = CalculateTimeWindowFromTimestamp(timestamp);
            return (int)this.Compute(window);
        }

        /// <summary>
        /// Takes a timestamp and computes a TOTP value for UTC now
        /// </summary>
        /// <returns>a TOTP value</returns>
        public int ComputeTotp()
        {
            return this.ComputeTotp(DateTime.UtcNow);
        }

        /// <summary>
        /// Verify a value that has been provided with the calculated value
        /// </summary>
        /// <param name="totp">the trial TOTP value</param>
        /// <param name="timeWindowUsed">This is an output parameter that gives that time window that was used to find a match.  This is usefule in cases where a TOTP value should only be used once.  This value is a unique identifier of the code slot (not the value) that can be used to prevent the same slot from being used multiple times</param>
        /// <param name="window">The window to verify</param>
        /// <returns>True if there is a match.</returns>
        public bool VerifyTotp(int totp, out long timeWindowUsed, VerificationWindow window = null)
        {
            return this.VerifyTotp(DateTime.UtcNow, totp, out timeWindowUsed, window);
        }

        /// <summary>
        /// Verify a value that has been provided with the calculated value
        /// </summary>
        /// <param name="timestamp">The timestamp to use</param>
        /// <param name="totp">the trial TOTP value</param>
        /// <param name="timeWindowUsed">
        /// This is an output parameter that gives that time window that was used to find a match.
        /// This is usefule in cases where a TOTP value should only be used once.  This value is a unique identifier of the
        /// code slot (not the value) that can be used to prevent the same slot from being used multiple times
        /// </param>
        /// <param name="window">The window to verify</param>
        /// <returns>True if there is a match.</returns>
        public bool VerifyTotp(DateTime timestamp, int totp, out long timeWindowUsed, VerificationWindow window = null)
        {
            var initialFrame = CalculateTimeWindowFromTimestamp(timestamp);

            return this.Verify(totp, window, initialFrame, out timeWindowUsed);
        }

        /// <summary>
        /// Takes a timestamp and calculates a time window
        /// </summary>
        private long CalculateTimeWindowFromTimestamp(DateTime timestamp)
        {
            var unixTimestamp = (timestamp.Ticks - unixEpochTicks) / ticksToSeconds;
            var window = unixTimestamp / (long)this.step;
            return window;
        }

        /// <summary>
        /// Remaining seconds in current window based on UtcNow
        /// </summary>
        /// <returns>Number of remaining seconds</returns>
        public int RemainingSeconds()
        {
            return RemainingSeconds(DateTime.UtcNow);
        }

        /// <summary>
        /// Remaining seconds in current window
        /// </summary>
        /// <param name="timestamp">The timestamp</param>
        /// <returns>Number of remaining seconds</returns>
        public int RemainingSeconds(DateTime timestamp)
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
            var data = this.GetBigEndianBytes(counter);
            var otp = this.CalculateOtp(this.secretKey, data, this.hashMode);
            return this.Digits(otp, this.totpSize);
        }
    }
}