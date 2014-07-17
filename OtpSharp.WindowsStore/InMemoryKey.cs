using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtpSharp
{
    public class InMemoryKey : IKeyProvider
    {
        public InMemoryKey(byte[] secret)
        { }

        byte[] IKeyProvider.ComputeHmac(OtpHashMode mode, byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
