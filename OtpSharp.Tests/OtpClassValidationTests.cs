using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OtpSharp.Tests
{
    [TestClass]
    public class OtpClassValidationTests
    {
        [TestMethod]
        public void ContractTestKeySize_Success()
        {
            var t = new Totp(OtpCalculationTests.rfcTestKey);
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
            var t = new Totp(OtpCalculationTests.rfcTestKey, step: 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void StepSize_Negative()
        {
            var t = new Totp(OtpCalculationTests.rfcTestKey, step: -1);
        }

        [TestMethod]
        public void StepSize_Fifteen()
        {
            var t = new Totp(OtpCalculationTests.rfcTestKey, step: 15);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Digits_Zero()
        {
            var t = new Totp(OtpCalculationTests.rfcTestKey, totpSize: 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Digits_Negative()
        {
            var t = new Totp(OtpCalculationTests.rfcTestKey, totpSize: -1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Digits_Eleven()
        {
            var t = new Totp(OtpCalculationTests.rfcTestKey, totpSize: 11);
        }

        [TestMethod]
        public void Digits_Ten()
        {
            var t = new Totp(OtpCalculationTests.rfcTestKey, totpSize: 10);
        }

        [TestMethod]
        public void TotpTypeProperty()
        {
            var totp= new Totp(OtpCalculationTests.rfcTestKey);
            Assert.AreEqual("totp", totp.GetOtpType());
        }

        [TestMethod]
        public void HotpTypeProperty()
        {
            var hotp = new Hotp(OtpCalculationTests.rfcTestKey);
            Assert.AreEqual("hotp", hotp.GetOtpType());
        }

        [TestMethod]
        public void TotpUrl()
        {
            var hotp = new Totp(OtpCalculationTests.rfcTestKey);
            var url = hotp.GetKeyUrl("user");
            Assert.AreEqual(string.Format("otpauth://totp/user?secret={0}", Base32.Encode(OtpCalculationTests.rfcTestKey)), url);
        }
    }
}
