using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OtpSharp.Tests
{
    [TestClass]
    public class ProtectedKeyTests
    {
        [TestMethod]
        public void ProtectedKey_Basic()
        {
            var pk = new ProtectedKey(OtpCalculationTests.rfcTestKey);
            pk.UsePlainKey(key => CollectionAssert.AreEqual(OtpCalculationTests.rfcTestKey, key));
        }

        [TestMethod]
        public void ProtectedKey_WipeReference()
        {
            var key = OtpCalculationTests.rfcTestKey;
            var pk = new ProtectedKey(key);
            CollectionAssert.AreNotEqual(OtpCalculationTests.rfcTestKey, key);
        }

        [TestMethod]
        public void ProtectedKey_SkipWipeReference()
        {
            var key = OtpCalculationTests.rfcTestKey;
            var pk = new ProtectedKey(key, wipeKeyReference: false);
            CollectionAssert.AreEqual(OtpCalculationTests.rfcTestKey, key);
        }

        [TestMethod]
        public void ProtectedKey_ProtectKey()
        {
            var originalKey = KeyGeneration.GenerateKey(16);
            var originalCopy = new byte[16];
            Array.Copy(originalKey, originalCopy, 16);
            CollectionAssert.AreEqual(originalKey, originalCopy);

            ProtectedMemory.Protect(originalCopy, MemoryProtectionScope.SameProcess);
            CollectionAssert.AreNotEqual(originalKey, originalCopy);

            var pk = new ProtectedKey(originalCopy, wipeKeyReference: false, isProtected: true);

            pk.UsePlainKey(key => CollectionAssert.AreEqual(originalKey, key));
        }

        [TestMethod]
        public void ProtectedKey_MultipleUse()
        {
            var originalKey = KeyGeneration.GenerateKey(16);
            var originalCopy = new byte[16];
            Array.Copy(originalKey, originalCopy, 16);
            CollectionAssert.AreEqual(originalKey, originalCopy);

            ProtectedMemory.Protect(originalCopy, MemoryProtectionScope.SameProcess);
            CollectionAssert.AreNotEqual(originalKey, originalCopy);

            var pk = new ProtectedKey(originalCopy, wipeKeyReference: false, isProtected: true);

            // The key is protected and un-protected several times.
            // Make sure that the key can be used multiple times.
            for (int i = 0; i < 10; i++)
                pk.UsePlainKey(key => CollectionAssert.AreEqual(originalKey, key));
        }

        [TestMethod]
        public void ProtectedKey_ProtectKeyWithSpecificLength()
        {
            var originalKey = KeyGeneration.GenerateKey(20);
            var originalCopy = new byte[32];
            Array.Copy(originalKey, originalCopy, 20);

            ProtectedMemory.Protect(originalCopy, MemoryProtectionScope.SameProcess);
            CollectionAssert.AreNotEqual(originalKey, originalCopy);

            var pk = new ProtectedKey(originalCopy, wipeKeyReference: false, isProtected: true, keyLength: 20);

            pk.UsePlainKey(key => CollectionAssert.AreEqual(originalKey, key, "The unprotected plain key and the original key don't match"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ProtectedKey_UseKeyWithNullDelegate()
        {
            var pk = new ProtectedKey(OtpCalculationTests.rfcTestKey);
            pk.UsePlainKey(null);
        }
    }
}
