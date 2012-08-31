using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OtpSharp.Tests
{
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

        /// <remarks>
        /// Warning! This is potentially a brittle test as it relies on the completion of the test body within a specific time.
        /// 500 ms should be plenty but it is still a brittle test.
        /// </remarks>
        [TestMethod]
        public void TimeCorrection_Basic()
        {
            var hypotheticallyCorrectUtcTime = DateTime.UtcNow.AddMilliseconds(12500);
            var correction = new TimeCorrection(hypotheticallyCorrectUtcTime);

            var correctedTime = correction.CorrectedUtcNow;
            var difference = hypotheticallyCorrectUtcTime - correctedTime;

            Assert.IsTrue(Math.Abs(difference.TotalMilliseconds) <= 500, "The corrected value is wrong");
        }

        [TestMethod]
        public void TimeCorrection_UncorrectedInstanceSpecificReference()
        {
            var baseTime = DateTime.UtcNow;
            Assert.AreEqual(baseTime, TimeCorrection.UncorrectedInstance.GetCorrectedTime(baseTime));
        }
    }
}
