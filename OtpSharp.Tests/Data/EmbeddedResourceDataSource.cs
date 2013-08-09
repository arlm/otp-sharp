using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace OtpSharp.Tests.Data
{
    public abstract class EmbeddedResourceDataSource<T> : IEnumerable<T>
    {
        protected abstract string EmbededResource { get; }
        protected abstract T Fill(XElement e);
        protected abstract string RowElement { get; }

        T[] data;

        public EmbeddedResourceDataSource()
        {
            var assembly = typeof(EmbeddedResourceDataSource<T>).Assembly;
            using (var stream = assembly.GetManifestResourceStream(this.EmbededResource))
            {
                var doc = XDocument.Load(stream);
                var rows = from r in doc.Descendants(this.RowElement)
                           select this.Fill(r);
                this.data = rows.ToArray();
            }
        }

        private IEnumerable<T> dataElementsEnumerable
        {
            get
            {
                foreach (var d in this.data)
                    yield return d;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.dataElementsEnumerable.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.dataElementsEnumerable.GetEnumerator();
        }
    }
}
