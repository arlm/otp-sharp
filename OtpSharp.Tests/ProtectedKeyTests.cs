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
        [ExpectedException(typeof(ArgumentException))]
        public void ProtectedKey_Empty()
        {
            var key = new ProtectedKey(new byte[] { });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ProtectedKey_Null()
        {
            ProtectedKey key = new ProtectedKey(null);
        }

        [TestMethod]
        public void ProtectedKey_Basic()
        {
            var pk = new ProtectedKey(OtpCalculationTests.RfcTestKey);
            CollectionAssert.AreEqual(OtpCalculationTests.RfcTestKey, pk.GetCopyOfKey());
        }

        [TestMethod]
        public void ProtectedKey_WipeReference()
        {
            var key = OtpCalculationTests.RfcTestKey;
            var pk = ProtectedKey.CreateProtectedKeyAndDestroyPlaintextKey(key);
            CollectionAssert.AreNotEqual(OtpCalculationTests.RfcTestKey, key);
        }

        /// <remarks>
        /// This test exists because the original API would overwrite the plaintext key passed
        /// into the constructor with random garbage.  This test is to ensure that behavior
        /// isn't present anymore.
        /// </remarks>
        [TestMethod]
        public void ProtectedKey_EnsureOriginalkeyIntegrity()
        {
            var key = OtpCalculationTests.RfcTestKey;
            var pk = new ProtectedKey(key);
            CollectionAssert.AreEqual(OtpCalculationTests.RfcTestKey, key);
        }

        [TestMethod]
        public void ProtectedKey_ProtectKey()
        {
            var originalKey = KeyGeneration.GenerateRandomKey(16);
            var originalCopy = new byte[16];
            Array.Copy(originalKey, originalCopy, 16);
            CollectionAssert.AreEqual(originalKey, originalCopy);

            ProtectedMemory.Protect(originalCopy, MemoryProtectionScope.SameProcess);
            CollectionAssert.AreNotEqual(originalKey, originalCopy);

            var pk = ProtectedKey.CreateProtectedKeyFromPreProtectedMemory(originalCopy, 16, MemoryProtectionScope.SameProcess);

            CollectionAssert.AreEqual(originalKey, pk.GetCopyOfKey());
        }

        [TestMethod]
        public void ProtectedKey_ProtectKey_CrossProcess()
        {
            var originalKey = KeyGeneration.GenerateRandomKey(16);
            var originalCopy = new byte[16];
            Array.Copy(originalKey, originalCopy, 16);
            CollectionAssert.AreEqual(originalKey, originalCopy);

            ProtectedMemory.Protect(originalCopy, MemoryProtectionScope.CrossProcess);
            CollectionAssert.AreNotEqual(originalKey, originalCopy);

            var pk = ProtectedKey.CreateProtectedKeyFromPreProtectedMemory(originalCopy, 16, MemoryProtectionScope.CrossProcess);

            CollectionAssert.AreEqual(originalKey, pk.GetCopyOfKey());
        }

        [TestMethod]
        public void ProtectedKey_ProtectKey_SameLogon()
        {
            var originalKey = KeyGeneration.GenerateRandomKey(16);
            var originalCopy = new byte[16];
            Array.Copy(originalKey, originalCopy, 16);
            CollectionAssert.AreEqual(originalKey, originalCopy);

            ProtectedMemory.Protect(originalCopy, MemoryProtectionScope.SameLogon);
            CollectionAssert.AreNotEqual(originalKey, originalCopy);

            var pk = ProtectedKey.CreateProtectedKeyFromPreProtectedMemory(originalCopy, 16, MemoryProtectionScope.SameLogon);

            CollectionAssert.AreEqual(originalKey, pk.GetCopyOfKey());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ProtectedKey_ProtectKeyEmpty()
        {
            var pk = ProtectedKey.CreateProtectedKeyFromPreProtectedMemory(new byte[] { }, 16, MemoryProtectionScope.SameProcess);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ProtectedKey_ProtectKeyZeroLength()
        {
            var pk = ProtectedKey.CreateProtectedKeyFromPreProtectedMemory(OtpCalculationTests.RfcTestKey, 0, MemoryProtectionScope.SameProcess);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ProtectedKey_ProtectKeyNull()
        {
            var pk = ProtectedKey.CreateProtectedKeyFromPreProtectedMemory(null, 16, MemoryProtectionScope.SameProcess);
        }

        [TestMethod]
        public void ProtectedKey_MultipleUse()
        {
            var originalKey = KeyGeneration.GenerateRandomKey(16);
            var originalCopy = new byte[16];
            Array.Copy(originalKey, originalCopy, 16);
            CollectionAssert.AreEqual(originalKey, originalCopy);

            ProtectedMemory.Protect(originalCopy, MemoryProtectionScope.SameProcess);
            CollectionAssert.AreNotEqual(originalKey, originalCopy);

            var pk = ProtectedKey.CreateProtectedKeyFromPreProtectedMemory(originalCopy, 16, MemoryProtectionScope.SameProcess);

            // The key is protected and un-protected several times.
            // Make sure that the key can be used multiple times.
            for (int i = 0; i < 10; i++)
                CollectionAssert.AreEqual(originalKey, pk.GetCopyOfKey());
        }

        [TestMethod]
        public void ProtectedKey_ProtectKeyWithSpecificLength()
        {
            var originalKey = KeyGeneration.GenerateRandomKey(20);
            var originalCopy = new byte[32];
            Array.Copy(originalKey, originalCopy, 20);

            ProtectedMemory.Protect(originalCopy, MemoryProtectionScope.SameProcess);
            CollectionAssert.AreNotEqual(originalKey, originalCopy);

            var pk = ProtectedKey.CreateProtectedKeyFromPreProtectedMemory(originalCopy, 20, MemoryProtectionScope.SameProcess);

            CollectionAssert.AreEqual(originalKey, pk.GetCopyOfKey(), "The unprotected plain key and the original key don't match");
        }
    }
}
