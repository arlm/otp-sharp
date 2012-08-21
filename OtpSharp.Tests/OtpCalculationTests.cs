using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections.Generic;

namespace OtpSharp.Tests
{
    /// <summary>
    /// the RFC documentation provides a table of test values.  This class exercises all of those.
    /// </summary>
    /// <remarks>
    /// http://tools.ietf.org/html/rfc4226#appendix-D
    /// http://tools.ietf.org/html/rfc6238#appendix-B
    /// </remarks>
    [TestClass]
    public class OtpCalculationTests
    {
        /// <summary>
        /// This is the test key defined in the RFC
        /// </summary>
        public static byte[] rfcTestKey
        {
            get
            {
                return new byte[] { 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x30 };
            }
        }

        /// <summary>
        /// The test context
        /// </summary>
        public TestContext TestContext { get; set; }


        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Rfc4226AppendixD.xml",
            "Row",
            DataAccessMethod.Sequential)]
        public void OtpAppendixDTests()
        {
            // test values from RFC - Appendix D
            long counter = Convert.ToInt64(this.TestContext.DataRow["counter"]);
            long expectedResult = Convert.ToInt64(this.TestContext.DataRow["decimal"]);

            Hotp hotpCalculator = new Hotp(rfcTestKey);
            var otp = hotpCalculator.ComputeHotpDecimal(counter);

            Assert.AreEqual(expectedResult, otp);
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Rfc4226AppendixD.xml",
            "Row",
            DataAccessMethod.Sequential)]
        public void HotpAppendixDTests()
        {
            // test values from RFC - Appendix D
            long counter = Convert.ToInt64(this.TestContext.DataRow["counter"]);
            long expectedResult = Convert.ToInt32(this.TestContext.DataRow["hotp"]);

            Hotp hotpCalculator = new Hotp(rfcTestKey);
            var hotp = hotpCalculator.ComputeHotp(counter);

            Assert.AreEqual(expectedResult, hotp);
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Rfc6238AppendixB.xml",
            "Row",
            DataAccessMethod.Sequential)]
        public void TotpAppendixBTests()
        {
            // test values from RFC - Appendix D
            var time = DateTime.Parse((string)this.TestContext.DataRow["time"]);
            long expectedResult = Convert.ToInt32(this.TestContext.DataRow["totp"]);

            OtpHashMode mode;
            byte[] key;
            GetMode((string)this.TestContext.DataRow["mode"], out mode, out key);

            var totpCalculator = new Totp(key, mode: mode, totpSize: 8);
            var hotp = totpCalculator.ComputeTotp(time);

            Assert.AreEqual(expectedResult, hotp);
        }

        private void GetMode(string mode, out OtpHashMode outputMode, out byte[] key)
        {
            switch (mode)
            {
                case "SHA256":
                    outputMode = OtpHashMode.Sha256;
                    key = JoinKeys(32).ToArray();
                    break;
                case "SHA512":
                    outputMode = OtpHashMode.Sha512;
                    key = JoinKeys(64).ToArray();
                    break;
                case "SHA1":
                    outputMode = OtpHashMode.Sha1;
                    key = JoinKeys(20).ToArray();
                    break;
                default:
                    throw new Exception("Inavlid mode");
            }
        }

        /// <summary>
        /// Helper method to repeat the test key up to the number of bytes specified
        /// </summary>
        private IEnumerable<byte> JoinKeys(int bytes)
        {
            int i = 0;
            do
            {
                foreach (var b in rfcTestKey)
                {
                    yield return b;
                    i++;
                    if (i >= bytes)
                        break;
                }
            } while (i < bytes);
        }
    }
}