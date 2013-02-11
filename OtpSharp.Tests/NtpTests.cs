using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OtpSharp.Tests
{
    [TestClass]
    public class NtpTests
    {
        [TestMethod]
        public void NistParse_Success()
        {
            var response = @"
56328 13-02-05 18:41:11 00 0 0 248.8 UTC(NIST) * 
";
            DateTime time;
            Assert.IsTrue(Ntp.TryParseResponse(response, out time), "Couldn't parse time");
            Assert.AreEqual(new DateTime(2013, 2, 5, 18, 41, 11), time, "time doesn't match");
        }

        [TestMethod]
        public void NistParse_NoUtc()
        {
            DateTime time;
            Assert.IsFalse(Ntp.TryParseResponse("56328 13-02-05 18:41:11 00 0 0 248.8", out time), "parsed time");
            Assert.AreEqual(DateTime.MinValue, time);
        }

        [TestMethod]
        public void NistParse_NoRegexMatch()
        {
            DateTime time;
            Assert.IsFalse(Ntp.TryParseResponse("56328 13-02-05 18:4:1 00 0 0 248.8 UTC(NIST)", out time), "parsed time");
            Assert.AreEqual(DateTime.MinValue, time);
        }
    }
}
