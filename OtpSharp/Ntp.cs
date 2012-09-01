using System;
using System.IO;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OtpSharp
{
    /// <summary>
    /// Class to aide with NTP (network time protocol) time corrections
    /// </summary>
    /// <remarks>
    /// This is experimental and doesn't have great test coverage
    /// nor is this idea fully developed.
    /// This API may change
    /// </remarks>
    public static class Ntp
    {
        /// <summary>
        /// Get a time correction factor against NIST
        /// </summary>
        /// <returns>DateTimeOffset correction</returns>
        public static Task<TimeCorrection> GetTimeCorrectionFromNistAsync()
        {
            return (new TaskFactory<TimeCorrection>()).StartNew(() => GetTimeCorrectionFromNist());
        }

        /// <summary>
        /// Get a time correction factor against NIST
        /// </summary>
        /// <returns>DateTimeOffset correction</returns>
        public static TimeCorrection GetTimeCorrectionFromNist()
        {
            var servers = GetNistServers();

            foreach (string server in servers)
            {
                try
                {
                    string response = null;
                    using (var client = new TcpClient(server, 13))
                    {
                        var stream = client.GetStream();

                        using (var reader = new StreamReader(stream))
                        {
                            response = reader.ReadToEnd();
                        }
                    }
                    DateTime networkTime;
                    if (ParseResponse(response, out networkTime))
                    {
                        return new TimeCorrection(networkTime);
                    }
                }
                catch
                {
                    // LOG?
                    // Loop around and try again on a different endpoint
                }
            }

            throw new ApplicationException("Couldn't get network time");
        }

        private static string[] GetNistServers()
        {
            return new[]
            {
                "time.nist.gov", // round robbin

                "nist1-ny.ustiming.org",
                "nist1-nj.ustiming.org",
                "nist1-pa.ustiming.org",
                "time-a.nist.gov",
                "time-b.nist.gov",
                "nist1.aol-va.symmetricom.com",
                "nist1.columbiacountyga.gov",
                "nist1-atl.ustiming.org",
                "nist1-chi.ustiming.org",
                "nist-chicago (No DNS)",
                "nist.time.nosc.us",
                "nist.expertsmi.com",
                "nist.netservicesgroup.com",
                "nisttime.carsoncity.k12.mi.us",
                "nist1-lnk.binary.net",
                "wwv.nist.gov",
                "time-a.timefreq.bldrdoc.gov",
                "time-b.timefreq.bldrdoc.gov",
                "time-c.timefreq.bldrdoc.gov",
                "utcnist.colorado.edu",
                "utcnist2.colorado.edu",
                "ntp-nist.ldsbc.edu",
                "nist1-lv.ustiming.org",
                "time-nw.nist.gov",
                "nist-time-server.eoni.com",
                "nist1.aol-ca.symmetricom.com",
                "nist1.symmetricom.com",
                "nist1-sj.ustiming.org",
                "nist1-la.ustiming.org",
            };
        }

        const string pattern = @"([0-9]{2}\-[0-9]{2}\-[0-9]{2} [0-9]{2}:[0-9]{2}:[0-9]{2})";
        private static bool ParseResponse(string response, out DateTime ntpUtc)
        {
            if (response.ToUpperInvariant().Contains("UTC(NIST)"))
            {
                var match = Regex.Match(response, pattern);
                if (match.Success)
                {
                    ntpUtc = DateTime.Parse("20" + match.Groups[0].Value);
                    return true;
                }
                else
                {
                    ntpUtc = DateTime.MinValue;
                    return false;
                }
            }
            else
            {
                ntpUtc = DateTime.MinValue;
                return false;
            }
        }
    }
}