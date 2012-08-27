using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OtpSharp.Tests
{
    /// <summary>
    /// Tests to verify that the verification window works properly
    /// </summary>
    [TestClass]
    public class VerificationWindowTests
    {
        [TestMethod]
        public void InitialFrameOnlyNonZero()
        {
            VerificationWindow window = new VerificationWindow();
            VerifyContents(window, 5, 5);
        }

        [TestMethod]
        public void InitialFrameOnlyZero()
        {
            VerificationWindow window = new VerificationWindow();
            VerifyContents(window, 0, 0);
        }

        [TestMethod]
        public void ForwardAndBackFive()
        {
            VerificationWindow window = new VerificationWindow(5, 5);
            VerifyContents(window, 5, 5, 6, 7, 8, 9, 10, 4, 3, 2, 1, 0);
        }

        [TestMethod]
        public void ForwardAndBackFiveWithZeroTruncation()
        {
            VerificationWindow window = new VerificationWindow(5, 5);
            VerifyContents(window, 3, 4, 5, 6, 7, 8, 3, 2, 1, 0);
        }

        [TestMethod]
        public void RfcVerificationWindow()
        {
            VerifyContents(VerificationWindow.RfcSpecifiedNetworkDelay, 3, 2, 3, 4);
        }

        private void VerifyContents(VerificationWindow window, long initialFrame, params long[] contents)
        {
            Assert.AreEqual(contents.Length, contents.Distinct().Count(), "the VerificationWindow contents must be unique");
            Assert.IsTrue(contents.Any(s => s == initialFrame), "Must contain the initial frame");
            Assert.AreEqual(contents.Length, window.ValidationCandidates(initialFrame).Count());
        }
    }
}
