using FluentAssertions;
using NUnit.Framework;
using System;
using System.Security.Cryptography;

namespace OtpSharp.Tests
{
    [TestFixture]
    public class ProtectedKeyTests
    {
        [Test]
        public void ProtectedKey_Empty()
        {
            new Action(() => new InMemoryKey(new byte[] { }))
                .ShouldThrow<ArgumentException>()
                .WithMessage("The key must not be empty");
        }

        [Test]
        public void ProtectedKey_Null()
        {
            new Action(() => new InMemoryKey(null))
                .ShouldThrow<ArgumentNullException>()
                .WithMessage("Value cannot be null.\r\nParameter name: key");
        }

        [Test]
        public void ProtectedKey_Basic()
        {
            var pk = new InMemoryKey(OtpCalculationTests.RfcTestKey);
            CollectionAssert.AreEqual(OtpCalculationTests.RfcTestKey, pk.GetCopyOfKey());
        }

        [Test]
        public void ProtectedKey_WipeReference()
        {
            var key = OtpCalculationTests.RfcTestKey;
            var pk = InMemoryKey.CreateProtectedKeyAndDestroyPlaintextKey(key);
            CollectionAssert.AreNotEqual(OtpCalculationTests.RfcTestKey, key);
        }

        /// <remarks>
        /// This test exists because the original API would overwrite the plaintext key passed
        /// into the constructor with random garbage.  This test is to ensure that behavior
        /// isn't present anymore.
        /// </remarks>
        [Test]
        public void ProtectedKey_EnsureOriginalkeyIntegrity()
        {
            var key = OtpCalculationTests.RfcTestKey;
            var pk = new InMemoryKey(key);
            CollectionAssert.AreEqual(OtpCalculationTests.RfcTestKey, key);
        }

        [Test]
        public void ProtectedKey_ProtectKey()
        {
            var originalKey = KeyGeneration.GenerateRandomKey(16);
            var originalCopy = new byte[16];
            Array.Copy(originalKey, originalCopy, 16);
            CollectionAssert.AreEqual(originalKey, originalCopy);

            ProtectedMemory.Protect(originalCopy, MemoryProtectionScope.SameProcess);
            CollectionAssert.AreNotEqual(originalKey, originalCopy);

            var pk = InMemoryKey.CreateProtectedKeyFromPreProtectedMemory(originalCopy, 16, MemoryProtectionScope.SameProcess);

            CollectionAssert.AreEqual(originalKey, pk.GetCopyOfKey());
        }

        [Test]
        public void ProtectedKey_ProtectKey_CrossProcess()
        {
            var originalKey = KeyGeneration.GenerateRandomKey(16);
            var originalCopy = new byte[16];
            Array.Copy(originalKey, originalCopy, 16);
            CollectionAssert.AreEqual(originalKey, originalCopy);

            ProtectedMemory.Protect(originalCopy, MemoryProtectionScope.CrossProcess);
            CollectionAssert.AreNotEqual(originalKey, originalCopy);

            var pk = InMemoryKey.CreateProtectedKeyFromPreProtectedMemory(originalCopy, 16, MemoryProtectionScope.CrossProcess);

            CollectionAssert.AreEqual(originalKey, pk.GetCopyOfKey());
        }

        [Test]
        public void ProtectedKey_ProtectKey_SameLogon()
        {
            var originalKey = KeyGeneration.GenerateRandomKey(16);
            var originalCopy = new byte[16];
            Array.Copy(originalKey, originalCopy, 16);
            CollectionAssert.AreEqual(originalKey, originalCopy);

            ProtectedMemory.Protect(originalCopy, MemoryProtectionScope.SameLogon);
            CollectionAssert.AreNotEqual(originalKey, originalCopy);

            var pk = InMemoryKey.CreateProtectedKeyFromPreProtectedMemory(originalCopy, 16, MemoryProtectionScope.SameLogon);

            CollectionAssert.AreEqual(originalKey, pk.GetCopyOfKey());
        }

        [Test]
        public void ProtectedKey_ProtectKeyEmpty()
        {
            new Action(() => InMemoryKey.CreateProtectedKeyFromPreProtectedMemory(new byte[] { }, 16, MemoryProtectionScope.SameProcess))
                .ShouldThrow<ArgumentException>()
                .WithMessage("The key must not be empty");
        }

        [Test]
        public void ProtectedKey_ProtectKeyZeroLength()
        {
            new Action(() => InMemoryKey.CreateProtectedKeyFromPreProtectedMemory(OtpCalculationTests.RfcTestKey, 0, MemoryProtectionScope.SameProcess))
                .ShouldThrow<ArgumentException>()
                .WithMessage("The key must not be empty");
        }

        [Test]
        public void ProtectedKey_ProtectKeyNull()
        {
            new Action(() => InMemoryKey.CreateProtectedKeyFromPreProtectedMemory(null, 16, MemoryProtectionScope.SameProcess))
                .ShouldThrow<ArgumentNullException>()
                .WithMessage("Value cannot be null.\r\nParameter name: preProtectedKey");
        }

        [Test]
        public void ProtectedKey_MultipleUse()
        {
            var originalKey = KeyGeneration.GenerateRandomKey(16);
            var originalCopy = new byte[16];
            Array.Copy(originalKey, originalCopy, 16);
            CollectionAssert.AreEqual(originalKey, originalCopy);

            ProtectedMemory.Protect(originalCopy, MemoryProtectionScope.SameProcess);
            CollectionAssert.AreNotEqual(originalKey, originalCopy);

            var pk = InMemoryKey.CreateProtectedKeyFromPreProtectedMemory(originalCopy, 16, MemoryProtectionScope.SameProcess);

            // The key is protected and un-protected several times.
            // Make sure that the key can be used multiple times.
            for (int i = 0; i < 10; i++)
                CollectionAssert.AreEqual(originalKey, pk.GetCopyOfKey());
        }

        [Test]
        public void ProtectedKey_ProtectKeyWithSpecificLength()
        {
            var originalKey = KeyGeneration.GenerateRandomKey(20);
            var originalCopy = new byte[32];
            Array.Copy(originalKey, originalCopy, 20);

            ProtectedMemory.Protect(originalCopy, MemoryProtectionScope.SameProcess);
            CollectionAssert.AreNotEqual(originalKey, originalCopy);

            var pk = InMemoryKey.CreateProtectedKeyFromPreProtectedMemory(originalCopy, 20, MemoryProtectionScope.SameProcess);

            CollectionAssert.AreEqual(originalKey, pk.GetCopyOfKey(), "The unprotected plain key and the original key don't match");
        }
    }
}