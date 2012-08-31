using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OtpSharp.Tests
{
    /// <remarks>
    /// Warning! This is potentially a brittle test as it relies on the completion of the test body within a specific time.
    /// Every effort has been taken to make these tests not fail when they shouldn't.
    /// </remarks>
    [TestClass]
    public class TimeCorrectionTests
    {
        [TestMethod]
        public void TimeCorrection_BasicWithSpecificReference()
        {
            var baseTime = DateTime.UtcNow;
            var incorrectReferenceTime = baseTime.AddMilliseconds(-12500);

            var correction = new TimeCorrection(baseTime, incorrectReferenceTime);

            Assert.AreEqual(baseTime, correction.GetCorrectedTime(incorrectReferenceTime));
        }

        [TestMethod]
        public void TimeCorrection_Basic()
        {
            var hypotheticallyCorrectUtcTime = DateTime.UtcNow.AddMilliseconds(12500);
            var correction = new TimeCorrection(hypotheticallyCorrectUtcTime);

            var correctedTime = correction.CorrectedUtcNow;
            var difference = hypotheticallyCorrectUtcTime - correctedTime;

            Assert.IsTrue(Math.Abs(difference.TotalMilliseconds) <= 64, "The corrected value is wrong");
        }

        [TestMethod]
        public void TimeCorrection_UncorrectedInstanceSpecificReference()
        {
            var baseTime = DateTime.UtcNow;
            Assert.AreEqual(baseTime, TimeCorrection.UncorrectedInstance.GetCorrectedTime(baseTime));
        }

        [TestMethod]
        public void TimeCorrection_TotpWithCorrectionRemainingSeconds()
        {
            var correction = new TimeCorrection(DateTime.UtcNow.AddSeconds(5));

            var totp = new Totp(OtpCalculationTests.rfcTestKey);
            var correctedTotp = new Totp(OtpCalculationTests.rfcTestKey, timeCorrection: correction);

            var standardRemaining = totp.RemainingSeconds();
            var correctedRemaining = correctedTotp.RemainingSeconds();

            Assert.AreNotEqual(standardRemaining, correctedRemaining, "Correction wasn't applied");

            int difference = 0;
            if (standardRemaining > correctedRemaining)
                difference = standardRemaining - correctedRemaining;
            else
                difference = (standardRemaining + 30) - correctedRemaining;

            Assert.AreEqual(5, difference);
        }

        [TestMethod]
        public void TimeCorrection_TotpWithCorrectionGeneration()
        {
            var correction = new TimeCorrection(DateTime.UtcNow.AddSeconds(100)); // 100 ensures that at a minimum we are 3 steps away

            var correctedTotp = new Totp(OtpCalculationTests.rfcTestKey, timeCorrection: correction);
            var uncorrectedTotp = new Totp(OtpCalculationTests.rfcTestKey);

            // make sure we don't run over the window
            while (correctedTotp.RemainingSeconds() == 0)
                Thread.Sleep(100);

            // since the compute totp overload that takes a specific time doesn't apply correction we don't need to TOTP objects
            var uncorrectedCode = uncorrectedTotp.ComputeTotp(DateTime.UtcNow.AddSeconds(100));
            var correctedCode = correctedTotp.ComputeTotp();

            Assert.AreEqual(uncorrectedCode, correctedCode);
        }

        [TestMethod]
        public void TimeCorrection_TotpWithCorrectionVerification()
        {
            var correction = new TimeCorrection(DateTime.UtcNow.AddSeconds(100)); // 100 ensures that at a minimum we are 3 steps away

            var correctedTotp = new Totp(OtpCalculationTests.rfcTestKey, timeCorrection: correction);
            var uncorrectedTotp = new Totp(OtpCalculationTests.rfcTestKey);

            // make sure we don't run over the window
            while (correctedTotp.RemainingSeconds() == 0)
                Thread.Sleep(100);

            var code = correctedTotp.ComputeTotp();

            long correctedStep, uncorrectedStep;

            Assert.IsTrue(correctedTotp.VerifyTotp(code, out correctedStep));
            Assert.IsTrue(uncorrectedTotp.VerifyTotp(DateTime.UtcNow.AddSeconds(100), code, out uncorrectedStep));

            Assert.AreEqual(uncorrectedStep, correctedStep);
        }
    }
}
