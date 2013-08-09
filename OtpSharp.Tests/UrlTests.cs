using Base32;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Specialized;

namespace OtpSharp.Tests
{
    [TestFixture]
    public class UrlTests
    {
        #region to url

        [Test]
        public void TotpUrl()
        {
            var url = KeyUrl.GetTotpUrl(OtpCalculationTests.RfcTestKey, "user");
            Assert.AreEqual(string.Format("otpauth://totp/user?secret={0}", Base32Encoder.Encode(OtpCalculationTests.RfcTestKey)), url);
        }

        [Test]
        public void TotpUrl_Sha256()
        {
            var url = KeyUrl.GetTotpUrl(OtpCalculationTests.RfcTestKey, "user", mode: OtpHashMode.Sha256);
            Assert.AreEqual(string.Format("otpauth://totp/user?secret={0}&algorithm=Sha256", Base32Encoder.Encode(OtpCalculationTests.RfcTestKey)), url);
        }

        [Test]
        public void TotpUrl_Sha512()
        {
            var url = KeyUrl.GetTotpUrl(OtpCalculationTests.RfcTestKey, "user", mode: OtpHashMode.Sha512);
            Assert.AreEqual(string.Format("otpauth://totp/user?secret={0}&algorithm=Sha512", Base32Encoder.Encode(OtpCalculationTests.RfcTestKey)), url);
        }

        [Test]
        public void TotpUrl_StepSizeFifteen()
        {
            var url = KeyUrl.GetTotpUrl(OtpCalculationTests.RfcTestKey, "user", step: 15);
            Assert.AreEqual(string.Format("otpauth://totp/user?secret={0}&period=15", Base32Encoder.Encode(OtpCalculationTests.RfcTestKey)), url);
        }

        [Test]
        public void TotpUrl_DigitsEight()
        {
            var url = KeyUrl.GetTotpUrl(OtpCalculationTests.RfcTestKey, "user", totpSize: 8);
            Assert.AreEqual(string.Format("otpauth://totp/user?secret={0}&digits=8", Base32Encoder.Encode(OtpCalculationTests.RfcTestKey)), url);
        }

        [Test]
        public void TotpUrl_DigitsTen()
        {
            new Action(() => KeyUrl.GetTotpUrl(OtpCalculationTests.RfcTestKey, "user", totpSize: 10))
                .ShouldThrow<ArgumentException>()
                .WithMessage("size must be 6 or 8");
        }

        [Test]
        public void TotpUrl_Sha512AndStepSizeFifteen()
        {
            var url = KeyUrl.GetTotpUrl(OtpCalculationTests.RfcTestKey, "user", step: 15, mode: OtpHashMode.Sha512);
            Assert.AreEqual(string.Format("otpauth://totp/user?secret={0}&algorithm=Sha512&period=15", Base32Encoder.Encode(OtpCalculationTests.RfcTestKey)), url);
        }

        [Test]
        public void TotpUrl_Sha512AndStepSizeFifteenDigitsEight()
        {
            var url = KeyUrl.GetTotpUrl(OtpCalculationTests.RfcTestKey, "user", step: 15, mode: OtpHashMode.Sha512, totpSize: 8);
            Assert.AreEqual(string.Format("otpauth://totp/user?secret={0}&digits=8&algorithm=Sha512&period=15", Base32Encoder.Encode(OtpCalculationTests.RfcTestKey)), url);
        }

        [Test]
        public void TotpUrl_EmptyUser()
        {
            new Action(() => KeyUrl.GetTotpUrl(OtpCalculationTests.RfcTestKey, string.Empty))
                .ShouldThrow<ArgumentNullException>()
                .WithMessage("Value cannot be null.\r\nParameter name: user");
        }

        [Test]
        public void TotpUrl_NullUser()
        {
            new Action(() => KeyUrl.GetTotpUrl(OtpCalculationTests.RfcTestKey, null))
                .ShouldThrow<ArgumentNullException>()
                .WithMessage("Value cannot be null.\r\nParameter name: user");
        }

        [Test]
        public void TotpUrl_NullKey()
        {
            new Action(() => KeyUrl.GetTotpUrl(null, "user"))
                .ShouldThrow<ArgumentNullException>()
                .WithMessage("Value cannot be null.\r\nParameter name: key");
        }

        [Test]
        public void TotpUrl_EmptyKey()
        {
            new Action(() => KeyUrl.GetTotpUrl(new byte[] { }, "user"))
                .ShouldThrow<ArgumentNullException>()
                .WithMessage("Value cannot be null.\r\nParameter name: key");
        }

        [Test]
        public void HotpUrl()
        {
            var url = KeyUrl.GetHotpUrl(OtpCalculationTests.RfcTestKey, "user", 1);
            Assert.AreEqual(string.Format("otpauth://hotp/user?secret={0}&counter=1", Base32Encoder.Encode(OtpCalculationTests.RfcTestKey)), url);
        }

        [Test]
        public void HotpUrl_2()
        {
            var url = KeyUrl.GetHotpUrl(OtpCalculationTests.RfcTestKey, "user", 2);
            Assert.AreEqual(string.Format("otpauth://hotp/user?secret={0}&counter=2", Base32Encoder.Encode(OtpCalculationTests.RfcTestKey)), url);
        }

        [Test]
        public void HotpUrl_Digits8()
        {
            var url = KeyUrl.GetHotpUrl(OtpCalculationTests.RfcTestKey, "user", 2, 8);
            Assert.AreEqual(string.Format("otpauth://hotp/user?secret={0}&digits=8&counter=2", Base32Encoder.Encode(OtpCalculationTests.RfcTestKey)), url);
        }

        [Test]
        public void HotpUrl_DigitsTen()
        {
            new Action(() => KeyUrl.GetHotpUrl(OtpCalculationTests.RfcTestKey, "user", 1, 10))
                .ShouldThrow<ArgumentException>()
                .WithMessage("size must be 6 or 8");
        }

        #endregion to url

        #region from URL

        [Test]
        public void FromTotpUrl()
        {
            var url = string.Format("otpauth://totp/user?secret={0}", Base32Encoder.Encode(OtpCalculationTests.RfcTestKey));
            var otp = KeyUrl.FromUrl(url);
            Assert.IsTrue(otp is Totp);
            var totp = (Totp)otp;

            CollectionAssert.AreEqual(OtpCalculationTests.RfcTestKey, totp.GetKey(), "Key's don't match");
            Assert.AreEqual(6, totp.GetDigitLength(), "Digits don't match");
            Assert.AreEqual(30, totp.GetTimeStep(), "Step size doesn't match");
            Assert.AreEqual(OtpHashMode.Sha1, totp.GetHashMode(), "Hash mode doesn't match");
        }

        [Test]
        public void FromTotpUrl_ExplicitStep()
        {
            var url = string.Format("otpauth://totp/user?secret={0}&period=30", Base32Encoder.Encode(OtpCalculationTests.RfcTestKey));
            var otp = KeyUrl.FromUrl(url);
            Assert.IsTrue(otp is Totp);
            var totp = (Totp)otp;

            CollectionAssert.AreEqual(OtpCalculationTests.RfcTestKey, totp.GetKey(), "Key's don't match");
            Assert.AreEqual(6, totp.GetDigitLength(), "Digits don't match");
            Assert.AreEqual(30, totp.GetTimeStep(), "Step size doesn't match");
            Assert.AreEqual(OtpHashMode.Sha1, totp.GetHashMode(), "Hash mode doesn't match");
        }

        [Test]
        public void FromTotpUrl_StepInvalid()
        {
            var url = string.Format("otpauth://totp/user?secret={0}&period=a", Base32Encoder.Encode(OtpCalculationTests.RfcTestKey));
            new Action(() => KeyUrl.FromUrl(url)).ShouldThrow<ArgumentException>().WithMessage("Invalid digits , must be a number");
        }

        [Test]
        public void FromTotpUrl_NegativeStep()
        {
            var url = string.Format("otpauth://totp/user?secret={0}&period=-1", Base32Encoder.Encode(OtpCalculationTests.RfcTestKey));
            new Action(() => KeyUrl.FromUrl(url)).ShouldThrow<ArgumentException>().WithMessage("Invalid Period -1, must be at least 1");
        }

        [Test]
        public void FromTotpUrl_ZeroStep()
        {
            var url = string.Format("otpauth://totp/user?secret={0}&period=0", Base32Encoder.Encode(OtpCalculationTests.RfcTestKey));
            new Action(() => KeyUrl.FromUrl(url)).ShouldThrow<ArgumentException>().WithMessage("Invalid Period 0, must be at least 1");
        }

        [Test]
        public void FromTotpUrl_ExplicitHashMode()
        {
            var url = string.Format("otpauth://totp/user?secret={0}&algorithm=Sha1", Base32Encoder.Encode(OtpCalculationTests.RfcTestKey));
            var otp = KeyUrl.FromUrl(url);
            Assert.IsTrue(otp is Totp);
            var totp = (Totp)otp;

            CollectionAssert.AreEqual(OtpCalculationTests.RfcTestKey, totp.GetKey(), "Key's don't match");
            Assert.AreEqual(6, totp.GetDigitLength(), "Digits don't match");
            Assert.AreEqual(30, totp.GetTimeStep(), "Step size doesn't match");
            Assert.AreEqual(OtpHashMode.Sha1, totp.GetHashMode(), "Hash mode doesn't match");
        }

        [Test]
        public void FromTotpUrl_ExplicitHashModeInvalid()
        {
            var url = string.Format("otpauth://totp/user?secret={0}&algorithm=Garbage", Base32Encoder.Encode(OtpCalculationTests.RfcTestKey));
            new Action(() => KeyUrl.FromUrl(url)).ShouldThrow<ArgumentException>().WithMessage("Invalid Algorithm Garbage");
        }

        [Test]
        public void FromTotpUrl_Sha256()
        {
            var url = string.Format("otpauth://totp/user?secret={0}&algorithm=Sha256", Base32Encoder.Encode(OtpCalculationTests.RfcTestKey));
            var otp = KeyUrl.FromUrl(url);
            Assert.IsTrue(otp is Totp);
            var totp = (Totp)otp;

            CollectionAssert.AreEqual(OtpCalculationTests.RfcTestKey, totp.GetKey(), "Key's don't match");
            Assert.AreEqual(6, totp.GetDigitLength(), "Digits don't match");
            Assert.AreEqual(30, totp.GetTimeStep(), "Step size doesn't match");
            Assert.AreEqual(OtpHashMode.Sha256, totp.GetHashMode(), "Hash mode doesn't match");
        }

        [Test]
        public void FromTotpUrl_Sha512()
        {
            var url = string.Format("otpauth://totp/user?secret={0}&algorithm=Sha512", Base32Encoder.Encode(OtpCalculationTests.RfcTestKey));
            var otp = KeyUrl.FromUrl(url);
            Assert.IsTrue(otp is Totp);
            var totp = (Totp)otp;

            CollectionAssert.AreEqual(OtpCalculationTests.RfcTestKey, totp.GetKey(), "Key's don't match");
            Assert.AreEqual(6, totp.GetDigitLength(), "Digits don't match");
            Assert.AreEqual(30, totp.GetTimeStep(), "Step size doesn't match");
            Assert.AreEqual(OtpHashMode.Sha512, totp.GetHashMode(), "Hash mode doesn't match");
        }

        [Test]
        public void FromTotpUrl_StepSizeFifteen()
        {
            var url = string.Format("otpauth://totp/user?secret={0}&period=15", Base32Encoder.Encode(OtpCalculationTests.RfcTestKey));
            var otp = KeyUrl.FromUrl(url);
            Assert.IsTrue(otp is Totp);
            var totp = (Totp)otp;

            CollectionAssert.AreEqual(OtpCalculationTests.RfcTestKey, totp.GetKey(), "Key's don't match");
            Assert.AreEqual(6, totp.GetDigitLength(), "Digits don't match");
            Assert.AreEqual(15, totp.GetTimeStep(), "Step size doesn't match");
            Assert.AreEqual(OtpHashMode.Sha1, totp.GetHashMode(), "Hash mode doesn't match");
        }

        [Test]
        public void FromTotpUrl_DigitsEight()
        {
            var url = string.Format("otpauth://totp/user?secret={0}&digits=8", Base32Encoder.Encode(OtpCalculationTests.RfcTestKey));
            var otp = KeyUrl.FromUrl(url);
            Assert.IsTrue(otp is Totp);
            var totp = (Totp)otp;

            CollectionAssert.AreEqual(OtpCalculationTests.RfcTestKey, totp.GetKey(), "Key's don't match");
            Assert.AreEqual(8, totp.GetDigitLength(), "Digits don't match");
            Assert.AreEqual(30, totp.GetTimeStep(), "Step size doesn't match");
            Assert.AreEqual(OtpHashMode.Sha1, totp.GetHashMode(), "Hash mode doesn't match");
        }

        [Test]
        public void FromTotpUrl_DigitsTen()
        {
            var url = string.Format("otpauth://totp/user?secret={0}&digits=10", Base32Encoder.Encode(OtpCalculationTests.RfcTestKey));
            new Action(() => KeyUrl.FromUrl(url)).ShouldThrow<ArgumentException>().WithMessage("Invalid Digits 10, must be 6 or 8");
        }

        [Test]
        public void FromTotpUrl_DigitsInvalid()
        {
            var url = string.Format("otpauth://totp/user?secret={0}&digits=a", Base32Encoder.Encode(OtpCalculationTests.RfcTestKey));
            new Action(() => KeyUrl.FromUrl(url)).ShouldThrow<ArgumentException>().WithMessage("Invalid digits a, must be a number");
        }

        [Test]
        public void FromTotpUrl_Sha512AndStepSizeFifteen()
        {
            var url = string.Format("otpauth://totp/user/?secret={0}&algorithm=Sha512&period=15", Base32Encoder.Encode(OtpCalculationTests.RfcTestKey));
            var otp = KeyUrl.FromUrl(url);
            Assert.IsTrue(otp is Totp);
            var totp = (Totp)otp;

            CollectionAssert.AreEqual(OtpCalculationTests.RfcTestKey, totp.GetKey(), "Key's don't match");
            Assert.AreEqual(6, totp.GetDigitLength(), "Digits don't match");
            Assert.AreEqual(15, totp.GetTimeStep(), "Step size doesn't match");
            Assert.AreEqual(OtpHashMode.Sha512, totp.GetHashMode(), "Hash mode doesn't match");
        }

        [Test]
        public void FromTotpUrl_Sha512AndStepSizeFifteenInverseOrder()
        {
            var url = string.Format("otpauth://totp/user?secret={0}&period=15&algorithm=Sha512", Base32Encoder.Encode(OtpCalculationTests.RfcTestKey));
            var otp = KeyUrl.FromUrl(url);
            Assert.IsTrue(otp is Totp);
            var totp = (Totp)otp;

            CollectionAssert.AreEqual(OtpCalculationTests.RfcTestKey, totp.GetKey(), "Key's don't match");
            Assert.AreEqual(6, totp.GetDigitLength(), "Digits don't match");
            Assert.AreEqual(15, totp.GetTimeStep(), "Step size doesn't match");
            Assert.AreEqual(OtpHashMode.Sha512, totp.GetHashMode(), "Hash mode doesn't match");
        }

        [Test]
        public void FromTotpUrl_Sha512AndStepSizeFifteenDigitsEight()
        {
            var url = string.Format("otpauth://totp/user/?secret={0}&algorithm=Sha512&period=15&digits=8", Base32Encoder.Encode(OtpCalculationTests.RfcTestKey));
            var otp = KeyUrl.FromUrl(url);
            Assert.IsTrue(otp is Totp);
            var totp = (Totp)otp;

            CollectionAssert.AreEqual(OtpCalculationTests.RfcTestKey, totp.GetKey(), "Key's don't match");
            Assert.AreEqual(8, totp.GetDigitLength(), "Digits don't match");
            Assert.AreEqual(15, totp.GetTimeStep(), "Step size doesn't match");
            Assert.AreEqual(OtpHashMode.Sha512, totp.GetHashMode(), "Hash mode doesn't match");
        }

        [Test]
        public void FromTotpUrl_EmptyUrl()
        {
            new Action(() => KeyUrl.FromUrl(string.Empty)).ShouldThrow<ArgumentNullException>().WithMessage("Value cannot be null.\r\nParameter name: rawUrl");
        }

        [Test]
        public void FromTotpUrl_NullUrl()
        {
            new Action(() => KeyUrl.FromUrl(null)).ShouldThrow<ArgumentNullException>().WithMessage("Value cannot be null.\r\nParameter name: rawUrl");
        }

        [Test]
        public void FromTotpUrl_InvalidQueryString()
        {
            var url = string.Format("otpauth://totp/user?secret={0}&algorithm=Sha512&period=15&digits=8&blah=b", Base32Encoder.Encode(OtpCalculationTests.RfcTestKey));
            new Action(() => KeyUrl.FromUrl(url)).ShouldThrow<ArgumentException>().WithMessage("Invalid parameter in query string");
        }

        [Test]
        public void FromTotpUrl_MalFormattedUrl_NoUser()
        {
            var url = string.Format("otpauth://totp?secret={0}&algorithm=Sha512&period=15&digits=8", Base32Encoder.Encode(OtpCalculationTests.RfcTestKey));
            new Action(() => KeyUrl.FromUrl(url)).ShouldThrow<ArgumentException>().WithMessage("rawUrl is invalid");
        }

        [Test]
        public void FromTotpUrl_MalFormattedUrl_NoUserWithSlash()
        {
            var url = string.Format("otpauth://totp/?secret={0}&algorithm=Sha512&period=15&digits=8", Base32Encoder.Encode(OtpCalculationTests.RfcTestKey));
            new Action(() => KeyUrl.FromUrl(url)).ShouldThrow<ArgumentException>().WithMessage("rawUrl is invalid");
        }

        [Test]
        public void FromTotpUrl_MalFormattedUrl_ExtraArgument()
        {
            var url = string.Format("otpauth://totp/user/extra?secret={0}&algorithm=Sha512&period=15&digits=8", Base32Encoder.Encode(OtpCalculationTests.RfcTestKey));
            new Action(() => KeyUrl.FromUrl(url)).ShouldThrow<ArgumentException>().WithMessage("rawUrl is invalid");
        }

        [Test]
        public void FromTotpUrl_MalFormattedUrl_ExtraArgumentWithSlash()
        {
            var url = string.Format("otpauth://totp/user/extra/?secret={0}&algorithm=Sha512&period=15&digits=8", Base32Encoder.Encode(OtpCalculationTests.RfcTestKey));
            new Action(() => KeyUrl.FromUrl(url)).ShouldThrow<ArgumentException>().WithMessage("rawUrl is invalid");
        }

        [Test]
        public void FromTotpUrl_InvalidScheme()
        {
            var url = string.Format("otp://totp/user?secret={0}&algorithm=Sha512&period=15&digits=8", Base32Encoder.Encode(OtpCalculationTests.RfcTestKey));
            new Action(() => KeyUrl.FromUrl(url)).ShouldThrow<ArgumentException>().WithMessage("invalid scheme otp. Must be otpauth://");
        }

        [Test]
        public void FromTotpUrl_NoSecret()
        {
            var url = "otpauth://totp/user?period=30";
            new Action(() => KeyUrl.FromUrl(url)).ShouldThrow<ArgumentException>().WithMessage("must contain secret");
        }

        [Test]
        public void FromTotpUrl_NoQueryString()
        {
            var url = "otpauth://totp/user";
            new Action(() => KeyUrl.FromUrl(url)).ShouldThrow<ArgumentException>().WithMessage("Must have a query string");
        }

        [Test]
        public void FromTotpUrl_WithCounter()
        {
            var url = string.Format("otpauth://totp/user?secret={0}&counter=1", Base32Encoder.Encode(OtpCalculationTests.RfcTestKey));
            new Action(() => KeyUrl.FromUrl(url)).ShouldThrow<ArgumentException>().WithMessage("Invalid parameter in query string");
        }

        [Test]
        public void FromHotpUrl_WithPeriod()
        {
            var url = string.Format("otpauth://Hotp/user?secret={0}&period=15", Base32Encoder.Encode(OtpCalculationTests.RfcTestKey));
            new Action(() => KeyUrl.FromUrl(url)).ShouldThrow<ArgumentException>().WithMessage("Invalid parameter in query string");
        }

        [Test]
        public void FromUrl_InvalidType()
        {
            var url = string.Format("otpauth://sotp/user?secret={0}", Base32Encoder.Encode(OtpCalculationTests.RfcTestKey));
            new Action(() => KeyUrl.FromUrl(url)).ShouldThrow<ArgumentException>().WithMessage("rawUrl contains an invalid operation sotp. Must be hotp or totp");
        }

        [Test]
        public void FromUrl_InvalidProtocol()
        {
            var url = string.Format("otp://totp/user?secret={0}", Base32Encoder.Encode(OtpCalculationTests.RfcTestKey));
            new Action(() => KeyUrl.FromUrl(url)).ShouldThrow<ArgumentException>().WithMessage("invalid scheme otp. Must be otpauth://");
        }

        #endregion

        #region validate query fields

        [Test]
        public void ValidateQueryStringFields_NullCollection()
        {
            new Action(() => KeyUrl.ValidateQueryStringFields(null, "true")).ShouldThrow<ArgumentException>().WithMessage("Value cannot be null.\r\nParameter name: query");
        }

        [Test]
        public void ValidateQueryStringFields_EmptyCollection()
        {
            NameValueCollection collection = new NameValueCollection();
            Assert.IsTrue(KeyUrl.ValidateQueryStringFields(collection, "true"));
        }

        [Test]
        public void ValidateQueryStringFields_EmptyNull()
        {
            NameValueCollection collection = new NameValueCollection();
            Assert.IsTrue(KeyUrl.ValidateQueryStringFields(collection));
        }

        [Test]
        public void ValidateQueryStringFields_SingleNone()
        {
            NameValueCollection collection = new NameValueCollection();
            collection.Add("key", "value");
            Assert.IsFalse(KeyUrl.ValidateQueryStringFields(collection));
        }

        [Test]
        public void ValidateQueryStringFields_SingleTrue()
        {
            NameValueCollection collection = new NameValueCollection();
            collection.Add("true", "value");
            Assert.IsTrue(KeyUrl.ValidateQueryStringFields(collection, "true"));
        }

        [Test]
        public void ValidateQueryStringFields_SingleFalse()
        {
            NameValueCollection collection = new NameValueCollection();
            collection.Add("key", "value");
            Assert.IsFalse(KeyUrl.ValidateQueryStringFields(collection, "true"));
        }

        #endregion
    }
}