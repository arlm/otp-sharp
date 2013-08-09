using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace OtpSharp.Tests
{
    [TestFixture]
    public class KeyUtilityTests
    {
        [Test]
        public void BigEndianInt()
        {
            var data = KeyUtilities.GetBigEndianBytes(1);
            Assert.AreEqual(4, data.Length);
            foreach (var bte in data.Take(3))
                Assert.AreEqual(0x00, bte);

            Assert.AreEqual(0x01, data.Last());
        }

        [Test]
        public void BigEndianLong()
        {
            var data = KeyUtilities.GetBigEndianBytes(1L);
            Assert.AreEqual(8, data.Length);
            foreach (var bte in data.Take(7))
                Assert.AreEqual(0x00, bte);

            Assert.AreEqual(0x01, data.Last());
        }

        [Test]
        public void Destroy_NullArgument()
        {
            new Action(() => KeyUtilities.Destroy(null)).ShouldThrow<ArgumentNullException>().WithMessage("Value cannot be null.\r\nParameter name: sensetiveData");
        }

        [Test]
        public void Destroy_Empty()
        {
            KeyUtilities.Destroy(new byte[] { }); // just make sure this doesn't blow up
        }

        [Test]
        public void Destroy_Success()
        {
            var testKey = OtpCalculationTests.RfcTestKey;
            KeyUtilities.Destroy(testKey);
            CollectionAssert.AreNotEqual(OtpCalculationTests.RfcTestKey, testKey);
        }
    }
}