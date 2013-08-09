using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OtpSharp.Tests.Data
{
    public class Rfc6238AppendixBDataSource : EmbeddedResourceDataSource<Rfc6238AppendixBData>
    {
        protected override string EmbededResource { get { return "OtpSharp.Tests.Data.Rfc6238AppendixB.xml"; } }
        protected override string RowElement { get { return "Row"; } }

        protected override Rfc6238AppendixBData Fill(System.Xml.Linq.XElement e)
        {
            return new Rfc6238AppendixBData(e.Descendants("totp").First().Value,
                e.Descendants("mode").First().Value,
                e.Descendants("time").First().Value);
        }
    }
}
