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
            var pk = new ProtectedKey(OtpCalculationTests.rfcTestKey);
            pk.UsePlainKey(key => CollectionAssert.AreEqual(OtpCalculationTests.rfcTestKey, key));
        }

        [TestMethod]
        public void ProtectedKey_WipeReference()
        {
            var key = OtpCalculationTests.rfcTestKey;
            var pk = ProtectedKey.CreateProtectedKeyAndDestroyPlaintextKey(key);
            CollectionAssert.AreNotEqual(OtpCalculationTests.rfcTestKey, key);
        }

        /// <remarks>
        /// This test exists because the original API would overwrite the plaintext key passed
        /// into the constructor with random garbage.  This test is to ensure that behavior
        /// isn't present anymore.
        /// </remarks>
        [TestMethod]
        public void ProtectedKey_EnsureOriginalkeyIntegrity()
        {
            var key = OtpCalculationTests.rfcTestKey;
            var pk = new ProtectedKey(key);
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

            var pk = ProtectedKey.CreateProtectedKeyFromPreprotectedMemory(originalCopy, 16, MemoryProtectionScope.SameProcess);

            pk.UsePlainKey(key => CollectionAssert.AreEqual(originalKey, key));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ProtectedKey_ProtectKeyEmpty()
        {
            var pk = ProtectedKey.CreateProtectedKeyFromPreprotectedMemory(new byte[] { }, 16, MemoryProtectionScope.SameProcess);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ProtectedKey_ProtectKeyNull()
        {
            var pk = ProtectedKey.CreateProtectedKeyFromPreprotectedMemory(null, 16, MemoryProtectionScope.SameProcess);
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

            var pk = ProtectedKey.CreateProtectedKeyFromPreprotectedMemory(originalCopy, 16, MemoryProtectionScope.SameProcess);

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

            var pk = ProtectedKey.CreateProtectedKeyFromPreprotectedMemory(originalCopy, 20, MemoryProtectionScope.SameProcess);

            pk.UsePlainKey(key => CollectionAssert.AreEqual(originalKey, key, "The unprotected plain key and the original key don't match"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ProtectedKey_UseKeyWithNullDelegate()
        {
            var pk = new ProtectedKey(OtpCalculationTests.rfcTestKey);
            pk.UsePlainKey(null);
        }

        [TestMethod]
        public void ProtectedKey_UseKeyWipeTempKey()
        {
            var pk = new ProtectedKey(OtpCalculationTests.rfcTestKey);
            byte[] tempKey = null;
            pk.UsePlainKey(key =>
            {
                tempKey = key;
                CollectionAssert.AreEqual(OtpCalculationTests.rfcTestKey, tempKey);
            });

            CollectionAssert.AreNotEqual(OtpCalculationTests.rfcTestKey, tempKey);
        }

        [TestMethod]
        public void ProtectedKey_UseKeyThrowExceptionWipeKey()
        {
            var pk = new ProtectedKey(OtpCalculationTests.rfcTestKey);
            byte[] tempKey = null;

            try
            {
                pk.UsePlainKey(key =>
                {
                    tempKey = key;
                    CollectionAssert.AreEqual(OtpCalculationTests.rfcTestKey, tempKey);
                    throw new ArgumentNullException();
                });

                Assert.Fail("The exception should have diverted control away from here.");
            }
            catch (ArgumentNullException)
            {
                CollectionAssert.AreNotEqual(OtpCalculationTests.rfcTestKey, tempKey);
            }
        }
    }
}
