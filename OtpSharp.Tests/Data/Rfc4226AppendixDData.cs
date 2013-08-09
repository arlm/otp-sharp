using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OtpSharp.Tests.Data
{
    public class Rfc4226AppendixDData
    {
        readonly string hotpValue;
        readonly int counterValue;
        readonly long decimalValue;

        public Rfc4226AppendixDData(string hotp, string counter, string dec)
        {
            this.hotpValue = hotp;
            this.counterValue = int.Parse(counter);
            this.decimalValue = long.Parse(dec);
        }

        public string Hotp { get { return this.hotpValue; } }
        public int Counter { get { return this.counterValue; } }
        public long Decimal { get { return this.decimalValue; } }
    }
}
