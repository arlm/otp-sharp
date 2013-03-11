using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OtpSharp.Tests
{
    [TestClass]
    public class KeyGenerationTests
    {
        [TestMethod]
        public void LengthForMode_Sha1()
        {
            Assert.AreEqual(20, KeyGeneration.GenerateRandomKey(OtpHashMode.Sha1).Length);
        }

        [TestMethod]
        public void LengthForMode_Sha256()
        {
            Assert.AreEqual(32, KeyGeneration.GenerateRandomKey(OtpHashMode.Sha256).Length);
        }

        [TestMethod]
        public void LengthForMode_Sha512()
        {
            Assert.AreEqual(64, KeyGeneration.GenerateRandomKey(OtpHashMode.Sha512).Length);
        }

        [TestMethod]
        public void GenerageKey_Zero()
        {
            Assert.AreEqual(0, KeyGeneration.GenerateRandomKey(0).Length);
        }

        [TestMethod]
        public void GenerageKey_Ten()
        {
            Assert.AreEqual(10, KeyGeneration.GenerateRandomKey(10).Length);
        }

        [TestMethod]
        public void GenerateKeyFromMaster_1()
        {

        }
    }
}
