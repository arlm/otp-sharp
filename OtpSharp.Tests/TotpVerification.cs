using NUnit.Framework;
using System;
using System.Threading;

namespace OtpSharp.Tests
{
    [TestFixture]
    public class TotpVerification
    {
        DateTime testTime = new DateTime(2009, 2, 13, 23, 31, 30);

        [Test]
        public void ExactMatch_SpecificVerificationWindow()
        {
            var verificationWindow = new VerificationWindow();

            AssertWindow(verificationWindow);
        }

        [Test]
        public void ExactMatch_SpecificVerificationWindowWithPriors()
        {
            var verificationWindow = new VerificationWindow(previous: 1);

            AssertWindow(verificationWindow);
        }

        [Test]
        public void ExactMatch_NullVerificationWindow()
        {
            VerificationWindow verificationWindow = null;

            AssertWindow(verificationWindow);
        }

        [Test]
        public void PreviousMatch_SpecificVerificationWindow()
        {
            var verificationWindow = new VerificationWindow(previous: 1);

            AssertWindow(verificationWindow, -1);
        }

        [Test]
        public void PreviousNonMatch_SpecificVerificationWindow()
        {
            var verificationWindow = new VerificationWindow();

            AssertWindow(verificationWindow, -1, false);
        }

        [Test]
        public void FutureMatch_SpecificVerificationWindow()
        {
            var verificationWindow = new VerificationWindow(future: 1);

            AssertWindow(verificationWindow, 1);
        }

        [Test]
        public void FutureNonMatch_SpecificVerificationWindow()
        {
            var verificationWindow = new VerificationWindow();

            AssertWindow(verificationWindow, 1, false);
        }

        [Test]
        public void ExactMatch_SpecificVerificationWindowUtcTime()
        {
            var totp = new Totp(OtpCalculationTests.RfcTestKey);

            // we don't want this to cross over the time step window mid test.
            // if there is at least a second remaining we should be just fine
            while (totp.RemainingSeconds() == 0)
                Thread.Sleep(100);

            var code = totp.ComputeTotp();
            long window;
            Assert.IsTrue(totp.VerifyTotp(code, out window));
            Assert.IsTrue(window > 0);
        }

        private void AssertWindow(VerificationWindow verificationWindow, int i = 0, bool shouldMatch = true)
        {
            var time = testTime.AddSeconds(30 * i);

            var totp = new Totp(OtpCalculationTests.RfcTestKey);
            var expected = totp.ComputeTotp(time);
            long timeStepUsed;
            var success = totp.VerifyTotp(testTime, expected, out timeStepUsed, verificationWindow);
            if (shouldMatch)
                Assert.IsTrue(success);
            else
                Assert.IsFalse(success);
        }

        [Test]
        public void Totp_EnsureKeyIntegrity()
        {
            var key = OtpCalculationTests.RfcTestKey;
            var totp = new Totp(key);
            CollectionAssert.AreEqual(OtpCalculationTests.RfcTestKey, key);
        }

        [Test]
        public void TotpUtcOverload()
        {
            var totp = new Totp(OtpCalculationTests.RfcTestKey);

            // we don't want this test to barely hit the time window crossover.
            // If there is at least 1 remaining second we should be OK
            while (totp.RemainingSeconds() == 0)
                Thread.Sleep(100);

            var code1 = totp.ComputeTotp(DateTime.UtcNow);
            var code2 = totp.ComputeTotp();

            Assert.AreEqual(code1, code2);
        }

        [Test]
        public void TotpRemainingTimeUtcOverload()
        {
            var totp = new Totp(OtpCalculationTests.RfcTestKey);

            var remaining1 = totp.RemainingSeconds();
            var remaining2 = totp.RemainingSeconds(DateTime.UtcNow);

            Assert.AreEqual(remaining1, remaining2);
        }
    }
}
