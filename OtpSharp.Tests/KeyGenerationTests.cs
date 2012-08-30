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
            Assert.AreEqual(20, KeyGeneration.GenerateKey(OtpHashMode.Sha1).Length);
        }

        [TestMethod]
        public void LengthForMode_Sha256()
        {
            Assert.AreEqual(32, KeyGeneration.GenerateKey(OtpHashMode.Sha256).Length);
        }

        [TestMethod]
        public void LengthForMode_Sha512()
        {
            Assert.AreEqual(64, KeyGeneration.GenerateKey(OtpHashMode.Sha512).Length);
        }

        [TestMethod]
        public void GenerageKey_Zero()
        {
            Assert.AreEqual(0, KeyGeneration.GenerateKey(0).Length);
        }

        [TestMethod]
        public void GenerageKey_Ten()
        {
            Assert.AreEqual(10, KeyGeneration.GenerateKey(10).Length);
        }
    
    }
}
