using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OtpSharp.Tests
{
    /// <summary>
    /// We want to make assertions on some internal things. Rather than expose them for tests, we are reflectively pulling them out
    /// </summary>
    static class ReflectiveExtensions
    {
        public static int GetDigitLength(this Totp totp)
        {
            var field = typeof(Totp).GetField("totpSize", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(field, "the totpSize field must have been renamed, please update this test accordingly");

            object value = field.GetValue(totp);
            Assert.IsTrue(value is int, "the totpSize field must have changed types from an int, please update this test accordingly");
            return (int)value;
        }

        public static int GetTimeStep(this Totp totp)
        {
            var field = typeof(Totp).GetField("step", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(field, "the step field must have been renamed, please update this test accordingly");

            object value = field.GetValue(totp);
            Assert.IsTrue(value is int, "the step field must have changed types from an int, please update this test accordingly");
            return (int)value;
        }


        public static byte[] GetKey(this Otp otp)
        {
            var field = typeof(Otp).GetField("secretKey", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(field, "the secretKey field must have been renamed, please update this test accordingly");

            object value = field.GetValue(otp);
            Assert.IsTrue(value is InMemoryKey, "the secretKey field must have changed types from an InMemoryKey, please update this test accordingly");
            var inMemoryKey = (InMemoryKey)value;

            return inMemoryKey.GetCopyOfKey();
        }

        public static OtpHashMode GetHashMode(this Otp otp)
        {
            var field = typeof(Otp).GetField("hashMode", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(field, "the hashMode field must have been renamed, please update this test accordingly");

            object value = field.GetValue(otp);
            Assert.IsTrue(value is OtpHashMode, "the hashMode field must have changed types from an OtpHashMode, please update this test accordingly");
            return (OtpHashMode)value;
        }

    }
}
