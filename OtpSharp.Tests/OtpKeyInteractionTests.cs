using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace OtpSharp.Tests
{
    [TestClass]
    public class OtpKeyInteractionTests
    {
        private Mock<IKeyProvider> keyMock
        {
            get
            {
                var mockable = new Mock<IKeyProvider>();
                // setup the mock to just return the RFC test key as the computed hash no matter what.
                // This will ensure that the OTP type has something to format and won't blow up.
                mockable.Setup(x => x.ComputeHmac(It.IsAny<OtpHashMode>(), It.IsAny<byte[]>())).Returns(OtpCalculationTests.RfcTestKey);
                return mockable;
            }
        }

        private readonly DateTime testDate = new DateTime(2000, 1, 1);
        /// <summary>
        /// This is the data that should be passed into the compute hmac for the given date above
        /// </summary>
        private readonly byte[] testData = KeyUtilities.GetBigEndianBytes(31556160L);

        [TestMethod]
        public void Totp_Sha1_Called()
        {
            var mock = this.keyMock;
            IKeyProvider key = mock.Object;

            var totp = new Totp(key, mode: OtpHashMode.Sha1);
            totp.ComputeTotp(testDate);

            mock.Verify(k => k.ComputeHmac(OtpHashMode.Sha1, testData));
        }

        [TestMethod]
        public void Totp_Sha256_Called()
        {
            var mock = this.keyMock;
            IKeyProvider key = mock.Object;

            var totp = new Totp(key, mode: OtpHashMode.Sha256);
            totp.ComputeTotp(testDate);

            mock.Verify(k => k.ComputeHmac(OtpHashMode.Sha256, testData));
        }

        [TestMethod]
        public void Totp_Sha512_Called()
        {
            var mock = this.keyMock;
            IKeyProvider key = mock.Object;

            var totp = new Totp(key, mode: OtpHashMode.Sha512);
            totp.ComputeTotp(testDate);

            mock.Verify(k => k.ComputeHmac(OtpHashMode.Sha512, testData));
        }
    }
}
