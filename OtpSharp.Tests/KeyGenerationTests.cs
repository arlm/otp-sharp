using NUnit.Framework;

namespace OtpSharp.Tests
{
    [TestFixture]
    public class KeyGenerationTests
    {
        [Test]
        public void LengthForMode_Sha1()
        {
            Assert.AreEqual(20, KeyGeneration.GenerateRandomKey(OtpHashMode.Sha1).Length);
        }

        [Test]
        public void LengthForMode_Sha256()
        {
            Assert.AreEqual(32, KeyGeneration.GenerateRandomKey(OtpHashMode.Sha256).Length);
        }

        [Test]
        public void LengthForMode_Sha512()
        {
            Assert.AreEqual(64, KeyGeneration.GenerateRandomKey(OtpHashMode.Sha512).Length);
        }

        [Test]
        public void GenerageKey_Zero()
        {
            Assert.AreEqual(0, KeyGeneration.GenerateRandomKey(0).Length);
        }

        [Test]
        public void GenerageKey_Ten()
        {
            Assert.AreEqual(10, KeyGeneration.GenerateRandomKey(10).Length);
        }
    }
}
