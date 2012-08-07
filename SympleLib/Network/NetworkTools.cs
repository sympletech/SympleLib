using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace SympleLib.Network
{
    public class NetworkTools
    {
        public static string Ping(string hostNameOrAddress)
        {
            var p = new Ping();
            var pr = p.Send(hostNameOrAddress);

            return string.Format("Reply From {0}: RoundTrip: {1}ms Status: {2}",
                hostNameOrAddress, pr.RoundtripTime, pr.Status);
        }

        //ref: http://coding.infoconex.com/post/C-Traceroute-using-net-framework.aspx
        public static string Traceroute(string ipAddressOrHostName)
        {
            try
            {
                var ipAddress = Dns.GetHostEntry(ipAddressOrHostName).AddressList[0];
                var traceResults = new StringBuilder();
                using (var pingSender = new Ping())
                {
                    var pingOptions = new PingOptions();
                    var stopWatch = new Stopwatch();
                    pingOptions.DontFragment = true;
                    pingOptions.Ttl = 1;
                    const int maxHops = 30;
                    traceResults.AppendLine(
                        string.Format(
                            "Tracing route to {0} over a maximum of {1} hops:<br/>",
                            ipAddress,
                            maxHops));
                    traceResults.AppendLine();
                    for (int i = 1; i < maxHops + 1; i++)
                    {
                        stopWatch.Reset();
                        stopWatch.Start();
                        PingReply pingReply = pingSender.Send(
                            ipAddress,
                            5000,
                            new byte[32], pingOptions);
                        stopWatch.Stop();
                        traceResults.AppendLine(
                            string.Format("{0}\t{1} ms\t{2}<br/>",
                            i,
                            stopWatch.ElapsedMilliseconds,
                            pingReply.Address));
                        if (pingReply.Status == IPStatus.Success)
                        {
                            traceResults.AppendLine();
                            traceResults.AppendLine("Trace complete.<br/>"); break;
                        }
                        pingOptions.Ttl++;
                    }
                }
                return traceResults.ToString();
            }
            catch
            {
                return "Unable To Perform Trace Route";
            }
        }

        //Ref: http://msdn.microsoft.com/en-us/library/ms143997.aspx
        public static string NsLookup(string hostNameOrAddress)
        {
            try
            {
                IPHostEntry host = Dns.GetHostEntry(hostNameOrAddress);
                var sbResult = new StringBuilder();

                sbResult.Append("Performing NSLookup...</b><br/><br/>");
                sbResult.AppendFormat("Host Name: {0}<br/>", host.HostName);
                foreach (var ip in host.AddressList)
                {
                    sbResult.AppendFormat("IP Address: {0}<br/>", ip);
                }

                return sbResult.ToString();
            }
            catch
            {
                return "Unable To Perform NSLookup on " + hostNameOrAddress;
            }
        }
    }
}
