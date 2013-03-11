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
        public static byte[] RfcTestKey
        {
            get
            {
                // return a new key every time since some functionality will destroy the object referenc
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

            Hotp hotpCalculator = new Hotp(RfcTestKey);
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
            string expectedResult = (string)this.TestContext.DataRow["hotp"];

            Hotp hotpCalculator = new Hotp(RfcTestKey);
            var hotp = hotpCalculator.ComputeHotp(counter);

            Assert.AreEqual(expectedResult, hotp);
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Rfc4226AppendixD.xml",
            "Row",
            DataAccessMethod.Sequential)]
        public void HotpAppendixDTests_ProtectedKey()
        {
            // test values from RFC - Appendix D
            long counter = Convert.ToInt64(this.TestContext.DataRow["counter"]);
            string expectedResult = (string)this.TestContext.DataRow["hotp"];

            Hotp hotpCalculator = new Hotp(new ProtectedKey(RfcTestKey));
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
            string expectedResult = (string)this.TestContext.DataRow["totp"];

            OtpHashMode mode;
            byte[] key;
            GetMode((string)this.TestContext.DataRow["mode"], out mode, out key);

            var totpCalculator = new Totp(key, mode: mode, totpSize: 8);
            var hotp = totpCalculator.ComputeTotp(time);

            Assert.AreEqual(expectedResult, hotp);
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Rfc6238AppendixB.xml",
            "Row",
            DataAccessMethod.Sequential)]
        public void TotpAppendixBTests_ProtectedKey()
        {
            // test values from RFC - Appendix D
            var time = DateTime.Parse((string)this.TestContext.DataRow["time"]);
            string expectedResult = (string)this.TestContext.DataRow["totp"];

            OtpHashMode mode;
            byte[] key;
            GetMode((string)this.TestContext.DataRow["mode"], out mode, out key);

            var totpCalculator = new Totp(new ProtectedKey(key), mode: mode, totpSize: 8);
            var hotp = totpCalculator.ComputeTotp(time);

            Assert.AreEqual(expectedResult, hotp);
        }

        /// <summary>
        /// Ensures that the padding is correct
        /// </summary>
        [TestMethod]
        public void HotpPaddingTest()
        {
            var hotpCalculator = new Hotp(RfcTestKey);
            var hotp = hotpCalculator.ComputeHotp(25193);
            Assert.AreEqual("000039", hotp);
        }

        /// <summary>
        /// Ensures that the padding is correct
        /// </summary>
        [TestMethod]
        public void Totp8DigitPaddingTest()
        {
            var totpCalculator = new Totp(RfcTestKey, totpSize:8);
            var date = new DateTime(1970, 1, 19, 13, 23, 00);
            var totp = totpCalculator.ComputeTotp(date);
            Assert.AreEqual("00003322", totp);
        }

        /// <summary>
        /// Ensures that the padding is correct
        /// </summary>
        [TestMethod]
        public void Totp6DigitPaddingTest()
        {
            var totpCalculator = new Totp(RfcTestKey, totpSize: 6);
            var date = new DateTime(1970, 1, 19, 13, 23, 00);
            var totp = totpCalculator.ComputeTotp(date);
            Assert.AreEqual("003322", totp);
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
                foreach (var b in RfcTestKey)
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