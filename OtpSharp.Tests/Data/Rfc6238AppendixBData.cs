using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OtpSharp.Tests.Data
{
    public class Rfc6238AppendixBData
    {
        private readonly string totpValue;
        private readonly OtpHashMode modeValue;
        private readonly byte[] rfcTestKeyValue;
        private readonly DateTime timeValue;

        public Rfc6238AppendixBData(string totp, string mode, string time)
        {
            OtpHashMode hashMode;
            byte[] key;
            this.GetMode(mode, out hashMode, out key);
            this.modeValue = hashMode;
            this.rfcTestKeyValue = key;

            this.totpValue = totp;
            this.timeValue = DateTime.Parse(time);
        }
        
        public string Totp { get { return this.totpValue; } }
        public OtpHashMode Mode { get { return this.modeValue; } }
        public DateTime Time { get { return this.timeValue; } }
        public IEnumerable<byte> RfcTestKey
        {
            get
            {
                foreach (byte b in this.rfcTestKeyValue)
                    yield return b;
            }
        }

        private void GetMode(string mode, out OtpHashMode outputMode, out byte[] key)
        {
            switch (mode)
            {
                case "SHA256":
                    outputMode = OtpHashMode.Sha256;
                    key = JoinKeys(32).ToArray();
                    break;
                case "SHA512":
                    outputMode = OtpHashMode.Sha512;
                    key = JoinKeys(64).ToArray();
                    break;
                case "SHA1":
                    outputMode = OtpHashMode.Sha1;
                    key = JoinKeys(20).ToArray();
                    break;
                default:
                    throw new Exception("Inavlid mode");
            }
        }

        /// <summary>
        /// Helper method to repeat the test key up to the number of bytes specified
        /// </summary>
        private IEnumerable<byte> JoinKeys(int bytes)
        {
            int i = 0;
            do
            {
                foreach (var b in OtpCalculationTests.RfcTestKey)
                {
                    yield return b;
                    i++;
                    if (i >= bytes)
                        break;
                }
            } while (i < bytes);
        }
    }
}
