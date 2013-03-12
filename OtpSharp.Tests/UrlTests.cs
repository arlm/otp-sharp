using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OtpSharp.Tests
{
    [TestClass]
    public class UrlTests
    {
        [TestMethod]
        public void TotpUrl()
        {
            var url = KeyUrl.GetTotpUrl(OtpCalculationTests.RfcTestKey, "user");
            Assert.AreEqual(string.Format("otpauth://totp/user?secret={0}", Base32.Encode(OtpCalculationTests.RfcTestKey)), url);
        }

        [TestMethod]
        public void TotpUrl_Sha256()
        {
            var url = KeyUrl.GetTotpUrl(OtpCalculationTests.RfcTestKey, "user", mode: OtpHashMode.Sha256);
            Assert.AreEqual(string.Format("otpauth://totp/user?secret={0}&algorithm=Sha256", Base32.Encode(OtpCalculationTests.RfcTestKey)), url);
        }

        [TestMethod]
        public void TotpUrl_Sha512()
        {
            var url = KeyUrl.GetTotpUrl(OtpCalculationTests.RfcTestKey, "user", mode: OtpHashMode.Sha512);
            Assert.AreEqual(string.Format("otpauth://totp/user?secret={0}&algorithm=Sha512", Base32.Encode(OtpCalculationTests.RfcTestKey)), url);
        }

        [TestMethod]
        public void TotpUrl_StepSizeFifteen()
        {
            var url = KeyUrl.GetTotpUrl(OtpCalculationTests.RfcTestKey, "user", step: 15);
            Assert.AreEqual(string.Format("otpauth://totp/user?secret={0}&period=15", Base32.Encode(OtpCalculationTests.RfcTestKey)), url);
        }

        [TestMethod]
        public void TotpUrl_DigitsEight()
        {
            var url = KeyUrl.GetTotpUrl(OtpCalculationTests.RfcTestKey, "user", totpSize: 8);
            Assert.AreEqual(string.Format("otpauth://totp/user?secret={0}&digits=8", Base32.Encode(OtpCalculationTests.RfcTestKey)), url);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TotpUrl_DigitsTen()
        {
            var url = KeyUrl.GetTotpUrl(OtpCalculationTests.RfcTestKey, "user", totpSize: 10);
        }

        [TestMethod]
        public void TotpUrl_Sha512AndStepSizeFifteen()
        {
            var url = KeyUrl.GetTotpUrl(OtpCalculationTests.RfcTestKey, "user", step: 15, mode: OtpHashMode.Sha512);
            Assert.AreEqual(string.Format("otpauth://totp/user?secret={0}&algorithm=Sha512&period=15", Base32.Encode(OtpCalculationTests.RfcTestKey)), url);
        }

        [TestMethod]
        public void TotpUrl_Sha512AndStepSizeFifteenDigitsEight()
        {
            var url = KeyUrl.GetTotpUrl(OtpCalculationTests.RfcTestKey, "user", step: 15, mode: OtpHashMode.Sha512, totpSize: 8);
            Assert.AreEqual(string.Format("otpauth://totp/user?secret={0}&digits=8&algorithm=Sha512&period=15", Base32.Encode(OtpCalculationTests.RfcTestKey)), url);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TotpUrl_EmptyUser()
        {
            var url = KeyUrl.GetTotpUrl(OtpCalculationTests.RfcTestKey, string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TotpUrl_NullUser()
        {
            var url = KeyUrl.GetTotpUrl(OtpCalculationTests.RfcTestKey, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TotpUrl_NullKey()
        {
            var url = KeyUrl.GetTotpUrl(null, "user");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TotpUrl_EmptyKey()
        {
            var url = KeyUrl.GetTotpUrl(new byte[] { }, "user");
        }

        [TestMethod]
        public void HotpUrl()
        {
            var url = KeyUrl.GetHotpUrl(OtpCalculationTests.RfcTestKey, "user", 1);
            Assert.AreEqual(string.Format("otpauth://hotp/user?secret={0}&counter=1", Base32.Encode(OtpCalculationTests.RfcTestKey)), url);
        }

        [TestMethod]
        public void HotpUrl_2()
        {
            var url = KeyUrl.GetHotpUrl(OtpCalculationTests.RfcTestKey, "user", 2);
            Assert.AreEqual(string.Format("otpauth://hotp/user?secret={0}&counter=2", Base32.Encode(OtpCalculationTests.RfcTestKey)), url);
        }

        [TestMethod]
        public void HotpUrl_Digits8()
        {
            var url = KeyUrl.GetHotpUrl(OtpCalculationTests.RfcTestKey, "user", 2, 8);
            Assert.AreEqual(string.Format("otpauth://hotp/user?secret={0}&digits=8&counter=2", Base32.Encode(OtpCalculationTests.RfcTestKey)), url);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void HotpUrl_DigitsTen()
        {
            var url = KeyUrl.GetHotpUrl(OtpCalculationTests.RfcTestKey, "user", 1, 10);
        }
    }
}
