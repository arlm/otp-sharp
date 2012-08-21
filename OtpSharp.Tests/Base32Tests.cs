using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OtpSharp.Tests
{
    [TestClass]
    public class Base32Tests
    {
        public TestContext TestContext { get; set; }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Base32TestData.xml",
            "Row",
            DataAccessMethod.Sequential)]
        [TestMethod]
        public void Base32Encoding()
        {
            var base32 = this.TestContext.DataRow["base32"].ToString().ToUpperInvariant();
            var base64 = this.TestContext.DataRow["base64"].ToString();

            var sourceData = Convert.FromBase64String(base64);
            var convertedValue = Base32.Encode(sourceData);

            Assert.AreEqual(base32, convertedValue);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Base32Encoding_Null()
        {
            Base32.Encode(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Base32Decoding_Null()
        {
            Base32.Decode(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Base32Decoding_Empty()
        {
            Base32.Decode(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Base32Encoding_Empty()
        {
            Base32.Encode(new byte[] { });
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Base32TestData.xml",
            "Row",
            DataAccessMethod.Sequential)]
        [TestMethod]
        public void Base32Decoding()
        {
            var base32 = this.TestContext.DataRow["base32"].ToString();
            var base64 = this.TestContext.DataRow["base64"].ToString();

            var expectedValue = Convert.FromBase64String(base64);
            var convertedValue = Base32.Decode(base32);
            CollectionAssert.AreEqual(expectedValue, convertedValue);
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Base32TestData.xml",
            "Row",
            DataAccessMethod.Sequential)]
        [TestMethod]
        public void Base32Decoding_Unpadded()
        {
            var base32 = this.TestContext.DataRow["base32Unpadded"].ToString();
            var base64 = this.TestContext.DataRow["base64"].ToString();

            var expectedValue = Convert.FromBase64String(base64);
            var convertedValue = Base32.Decode(base32);
            CollectionAssert.AreEqual(expectedValue, convertedValue);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Base32Decoding_IllegalCharacters()
        {
            Base32.Decode("abcde10"); // the 1 and the 0 are illegal
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Base32Decoding_EmptyString()
        {
            Base32.Decode(string.Empty);
        }
    }
}
