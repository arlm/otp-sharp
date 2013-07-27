using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace OtpSharp.Tests
{
    [TestClass]
    public class KeyUtilityTests
    {
        [TestMethod]
        public void BigEndianInt()
        {
            var data = KeyUtilities.GetBigEndianBytes(1);
            Assert.AreEqual(4, data.Length);
            foreach (var bte in data.Take(3))
                Assert.AreEqual(0x00, bte);

            Assert.AreEqual(0x01, data.Last());
        }

        [TestMethod]
        public void BigEndianLong()
        {
            var data = KeyUtilities.GetBigEndianBytes(1L);
            Assert.AreEqual(8, data.Length);
            foreach (var bte in data.Take(7))
                Assert.AreEqual(0x00, bte);

            Assert.AreEqual(0x01, data.Last());
        }

        [TestMethod]
        public void Destroy_NullArgument()
        {
            new Action(() => KeyUtilities.Destroy(null)).ShouldThrow<ArgumentNullException>().WithMessage("Value cannot be null.\r\nParameter name: sensetiveData");
        }

        [TestMethod]
        public void Destroy_Empty()
        {
            KeyUtilities.Destroy(new byte[] { }); // just make sure this doesn't blow up
        }

        [TestMethod]
        public void Destroy_Success()
        {
            var testKey = OtpCalculationTests.RfcTestKey;
            KeyUtilities.Destroy(testKey);
            CollectionAssert.AreNotEqual(OtpCalculationTests.RfcTestKey, testKey);
        }
    }
}