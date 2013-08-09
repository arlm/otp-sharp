using Moq;
using NUnit.Framework;
using System;

namespace OtpSharp.Tests
{
    [TestFixture]
    public class OtpKeyInteractionTests
    {
        private Mock<IKeyProvider> keyMock
        {
            get
            {
                var mockable = new Mock<IKeyProvider>();
                // setup the mock to just return the RFC test key as the computed hash no matter what.
                // This will ensure that the OTP type has something to truncate and format and won't blow up.
                mockable.Setup(x => x.ComputeHmac(It.IsAny<OtpHashMode>(), It.IsAny<byte[]>())).Returns(OtpCalculationTests.RfcTestKey);
                return mockable;
            }
        }

        private readonly DateTime totpDate = new DateTime(2000, 1, 1);
        /// <summary>
        /// This is the data that should be passed into the compute hmac for the given date above
        /// </summary>
        private readonly byte[] totpData = KeyUtilities.GetBigEndianBytes(31556160L);

        private const long hotpCounter = 1;
        private readonly byte[] hotpData = KeyUtilities.GetBigEndianBytes(1L);

        [Test]
        public void Totp_Sha1_Default_Called()
        {
            var mock = this.keyMock;
            IKeyProvider key = mock.Object;

            var totp = new Totp(key);
            totp.ComputeTotp(totpDate);

            mock.Verify(k => k.ComputeHmac(OtpHashMode.Sha1, totpData));
        }

        [Test]
        public void Totp_Sha1_Called()
        {
            var mock = this.keyMock;
            IKeyProvider key = mock.Object;

            var totp = new Totp(key, mode: OtpHashMode.Sha1);
            totp.ComputeTotp(totpDate);

            mock.Verify(k => k.ComputeHmac(OtpHashMode.Sha1, totpData));
        }

        [Test]
        public void Totp_Sha256_Called()
        {
            var mock = this.keyMock;
            IKeyProvider key = mock.Object;

            var totp = new Totp(key, mode: OtpHashMode.Sha256);
            totp.ComputeTotp(totpDate);

            mock.Verify(k => k.ComputeHmac(OtpHashMode.Sha256, totpData));
        }

        [Test]
        public void Totp_Sha512_Called()
        {
            var mock = this.keyMock;
            IKeyProvider key = mock.Object;

            var totp = new Totp(key, mode: OtpHashMode.Sha512);
            totp.ComputeTotp(totpDate);

            mock.Verify(k => k.ComputeHmac(OtpHashMode.Sha512, totpData));
        }

        [Test]
        public void Hotp_Sha1_Default_Called()
        {
            var mock = this.keyMock;
            IKeyProvider key = mock.Object;

            var hotp = new Hotp(key);
            hotp.ComputeHotp(hotpCounter);

            mock.Verify(k => k.ComputeHmac(OtpHashMode.Sha1, hotpData));
        }

        [Test]
        public void Hotp_Sha1_Called()
        {
            var mock = this.keyMock;
            IKeyProvider key = mock.Object;

            var hotp = new Hotp(key, mode: OtpHashMode.Sha1);
            hotp.ComputeHotp(hotpCounter);

            mock.Verify(k => k.ComputeHmac(OtpHashMode.Sha1, hotpData));
        }

        [Test]
        public void Hotp_Sha256_Called()
        {
            var mock = this.keyMock;
            IKeyProvider key = mock.Object;

            var hotp = new Hotp(key, mode: OtpHashMode.Sha256);
            hotp.ComputeHotp(hotpCounter);

            mock.Verify(k => k.ComputeHmac(OtpHashMode.Sha256, hotpData));
        }

        [Test]
        public void Hotp_Sha512_Called()
        {
            var mock = this.keyMock;
            IKeyProvider key = mock.Object;

            var hotp = new Hotp(key, mode: OtpHashMode.Sha512);
            hotp.ComputeHotp(hotpCounter);

            mock.Verify(k => k.ComputeHmac(OtpHashMode.Sha512, hotpData));
        }
    }
}