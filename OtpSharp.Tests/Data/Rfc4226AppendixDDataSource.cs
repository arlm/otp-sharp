using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace OtpSharp.Tests.Data
{
    public class Rfc4226AppendixDDataSource : EmbeddedResourceDataSource<Rfc4226AppendixDData>
    {
        protected override string EmbededResource { get { return "OtpSharp.Tests.Data.Rfc4226AppendixD.xml"; } }
        protected override string RowElement { get { return "Row"; } }

        protected override Rfc4226AppendixDData Fill(XElement e)
        {
            return new Rfc4226AppendixDData(e.Descendants("hotp").First().Value, e.Descendants("counter").First().Value, e.Descendants("decimal").First().Value);
        }
    }
}
