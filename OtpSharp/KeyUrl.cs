using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace OtpSharp
{
#if NO_WEB
    // NO_WEB is useful in cases where only a client profile is available
#else
    /// <summary>
    /// Class that generates and reads the de-facto url format used by google
    /// </summary>
    public static class KeyUrl
    {
        /// <summary>
        /// Get a url for a TOTP key
        /// </summary>
        /// <param name="key">Plaintext key</param>
        /// <param name="user">The username</param>
        /// <param name="step">Timestep</param>
        /// <param name="mode">Hash mode</param>
        /// <param name="totpSize">Digits</param>
        /// <returns>URL</returns>
        public static string GetTotpUrl(byte[] key, string user, int step = 30, OtpHashMode mode = OtpHashMode.Sha1, int totpSize = 6)
        {
            var url = GetBaseKeyUrl(key, user, OtpType.Totp);

            if (mode != OtpHashMode.Sha1)
                url += string.Format(CultureInfo.InvariantCulture.NumberFormat, "&algorithm={0}", mode);

            if (step != 30)
                url += string.Format(CultureInfo.InvariantCulture.NumberFormat, "&period={0}", step);

            if (totpSize != 6)
            {
                if (totpSize == 8)
                    url += "&digits=8";
                else
                    throw new ArgumentException("The URL format doesn't allow for a digit size other than 8 or 6");
            }

            return url;
        }

        /// <summary>
        /// Gets a URL that conforms to the de-facto standard
        /// created and used by Google
        /// </summary>
        private static string GetBaseKeyUrl(byte[] key, string user, OtpType otpType)
        {
            if (string.IsNullOrEmpty(user))
                throw new ArgumentNullException("user");

            if (key == null || key.Length == 0)
                throw new ArgumentNullException("key");

            return string.Format("otpauth://{0}/{1}?secret={2}", otpType.ToString().ToLowerInvariant(), System.Web.HttpUtility.UrlEncode(user), Base32.Encode(key));
        }

    }
#endif
}
