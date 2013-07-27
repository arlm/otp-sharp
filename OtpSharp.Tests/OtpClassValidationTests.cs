using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace OtpSharp.Tests
{
    [TestClass]
    public class OtpClassValidationTests
    {
        [TestMethod]
        public void ContractTestKeySize_Success()
        {
            var t = new Totp(OtpCalculationTests.RfcTestKey);
        }

        [TestMethod]
        public void ContractTestKeySize_Null()
        {
            byte[] key = null;
            new Action(() => new Totp(key)).ShouldThrow<ArgumentNullException>().WithMessage("Value cannot be null.\r\nParameter name: secretKey"); ;
        }

        [TestMethod]
        public void ContractTestKeySize_InMemoryKeyNull()
        {
            InMemoryKey key = null;
            new Action(() => new Totp(key)).ShouldThrow<ArgumentNullException>().WithMessage("Value cannot be null.\r\nParameter name: secretKey"); ;
        }

        [TestMethod]
        public void ContractTestKeySize_Empty()
        {
            new Action(() => new Totp(new byte[] { })).ShouldThrow<ArgumentException>().WithMessage("secretKey empty");
        }

        [TestMethod]
        public void ContractTestKeySize_ProtectedKeyEmpty()
        {
            new Action(() => new Totp(new InMemoryKey(new byte[] { }))).ShouldThrow<ArgumentException>().WithMessage("The key must not be empty");
        }

        [TestMethod]
        public void StepSize_Zero()
        {
            new Action(() => new Totp(OtpCalculationTests.RfcTestKey, step: 0)).ShouldThrow<ArgumentOutOfRangeException>().WithMessage("Specified argument was out of the range of valid values.\r\nParameter name: step");
        }

        [TestMethod]
        public void StepSize_Negative()
        {
            new Action(() => new Totp(OtpCalculationTests.RfcTestKey, step: -1)).ShouldThrow<ArgumentOutOfRangeException>().WithMessage("Specified argument was out of the range of valid values.\r\nParameter name: step");
        }

        [TestMethod]
        public void StepSize_Fifteen()
        {
            var t = new Totp(OtpCalculationTests.RfcTestKey, step: 15);
        }

        [TestMethod]
        public void Digits_Zero()
        {
            new Action(() => new Totp(OtpCalculationTests.RfcTestKey, totpSize: 0)).ShouldThrow<ArgumentOutOfRangeException>().WithMessage("Specified argument was out of the range of valid values.\r\nParameter name: totpSize"); ;
        }

        [TestMethod]
        public void Digits_Negative()
        {
            new Action(() => new Totp(OtpCalculationTests.RfcTestKey, totpSize: -1)).ShouldThrow<ArgumentOutOfRangeException>().WithMessage("Specified argument was out of the range of valid values.\r\nParameter name: totpSize"); ;
        }

        [TestMethod]
        public void Digits_Eleven()
        {
            new Action(() => new Totp(OtpCalculationTests.RfcTestKey, totpSize: 11)).ShouldThrow<ArgumentOutOfRangeException>().WithMessage("Specified argument was out of the range of valid values.\r\nParameter name: totpSize"); ;
        }

        [TestMethod]
        public void Digits_Ten()
        {
            var t = new Totp(OtpCalculationTests.RfcTestKey, totpSize: 10);
        }
    }
}
