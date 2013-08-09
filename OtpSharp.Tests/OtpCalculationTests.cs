using NUnit.Framework;
using OtpSharp.Tests.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OtpSharp.Tests
{
    /// <summary>
    /// the RFC documentation provides a table of test values.  This class exercises all of those.
    /// </summary>
    /// <remarks>
    /// http://tools.ietf.org/html/rfc4226#appendix-D
    /// http://tools.ietf.org/html/rfc6238#appendix-B
    /// </remarks>
    [TestFixture]
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

        [Test]
        [TestCaseSource(typeof(Rfc4226AppendixDDataSource))]
        public void OtpAppendixDTests(Rfc4226AppendixDData data)
        {
            Assert.IsNotNull(data, "data was null");

            Hotp hotpCalculator = new Hotp(RfcTestKey);
            var otp = hotpCalculator.ComputeHotpDecimal(data.Counter, OtpHashMode.Sha1);

            Assert.AreEqual(data.Decimal, otp);
        }

        [Test]
        [TestCaseSource(typeof(Rfc4226AppendixDDataSource))]
        public void HotpAppendixDTests(Rfc4226AppendixDData data)
        {
            Assert.IsNotNull(data, "data was null");
            Hotp hotpCalculator = new Hotp(RfcTestKey);
            var hotp = hotpCalculator.ComputeHotp(data.Counter);
            Assert.AreEqual(data.Hotp, hotp);
        }

        [Test]
        [TestCaseSource(typeof(Rfc4226AppendixDDataSource))]
        public void HotpAppendixDTests_ProtectedKey(Rfc4226AppendixDData data)
        {
            Assert.IsNotNull(data, "data was null");

            Hotp hotpCalculator = new Hotp(new InMemoryKey(RfcTestKey));
            var hotp = hotpCalculator.ComputeHotp(data.Counter);

            Assert.AreEqual(data.Hotp, hotp);
        }

        [Test]
        [TestCaseSource(typeof(Rfc6238AppendixBDataSource))]
        public void TotpAppendixBTests(Rfc6238AppendixBData data)
        {
            Assert.IsNotNull(data, "data was null");
            var totpCalculator = new Totp(data.RfcTestKey.ToArray(), mode: data.Mode, totpSize: 8);
            var totp = totpCalculator.ComputeTotp(data.Time);

            Assert.AreEqual(data.Totp, totp);
        }

        [Test]
        [TestCaseSource(typeof(Rfc6238AppendixBDataSource))]
        public void TotpAppendixBTests_ProtectedKey(Rfc6238AppendixBData data)
        {
            Assert.IsNotNull(data, "data was null");

            var totpCalculator = new Totp(new InMemoryKey(data.RfcTestKey.ToArray()), mode: data.Mode, totpSize: 8);
            var totp = totpCalculator.ComputeTotp(data.Time);

            Assert.AreEqual(data.Totp, totp);
        }

        /// <summary>
        /// Ensures that the padding is correct
        /// </summary>
        [Test]
        public void HotpPaddingTest()
        {
            var hotpCalculator = new Hotp(RfcTestKey);
            var hotp = hotpCalculator.ComputeHotp(25193);
            Assert.AreEqual("000039", hotp);
        }

        /// <summary>
        /// Ensures that the padding is correct
        /// </summary>
        [Test]
        public void Totp8DigitPaddingTest()
        {
            var totpCalculator = new Totp(RfcTestKey, totpSize: 8);
            var date = new DateTime(1970, 1, 19, 13, 23, 00);
            var totp = totpCalculator.ComputeTotp(date);
            Assert.AreEqual("00003322", totp);
        }

        /// <summary>
        /// Ensures that the padding is correct
        /// </summary>
        [Test]
        public void Totp6DigitPaddingTest()
        {
            var totpCalculator = new Totp(RfcTestKey, totpSize: 6);
            var date = new DateTime(1970, 1, 19, 13, 23, 00);
            var totp = totpCalculator.ComputeTotp(date);
            Assert.AreEqual("003322", totp);
        }
    }
}