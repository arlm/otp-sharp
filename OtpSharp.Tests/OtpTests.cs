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
        public static readonly byte[] rfcTestKey = new byte[] { 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x30 };

        /// <summary>
        /// The test context
        /// </summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void ContractTestKeySize_Success()
        {
            var t = new Totp(rfcTestKey);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ContractTestKeySize_Null()
        {
            var t = new Totp(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ContractTestKeySize_Empty()
        {
            var t = new Totp(new byte[] { });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void StepSize_Zero()
        {
            var t = new Totp(rfcTestKey, step:0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void StepSize_Negative()
        {
            var t = new Totp(rfcTestKey, step: -1);
        }

        [TestMethod]
        public void StepSize_Fifteen()
        {
            var t = new Totp(rfcTestKey, step: 15);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Digits_Zero()
        {
            var t = new Totp(rfcTestKey, totpSize:0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Digits_Negative()
        {
            var t = new Totp(rfcTestKey, totpSize: -1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Digits_Eleven()
        {
            var t = new Totp(rfcTestKey, totpSize: 11);
        }
        
        [TestMethod]
        public void Digits_Ten()
        {
            var t = new Totp(rfcTestKey, totpSize: 10);
        }


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