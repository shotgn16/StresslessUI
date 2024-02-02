using System.Net;
using System.Net.NetworkInformation;

namespace StresslessUI.Net
{
    internal class Networks
    {
        public static async Task<string> getIPv4()
        {
            IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
            string IPAddress = "";
            
            foreach (var ipAddress in hostEntry.AddressList)
            {
                IPAddress = Convert.ToString(ipAddress);
            }

            return IPAddress;
        }

        public static async Task<string> getMAC()
        {
            return (from nic in NetworkInterface.GetAllNetworkInterfaces()
                   where nic.OperationalStatus == OperationalStatus.Up
                   select
                   nic.GetPhysicalAddress().ToString()).FirstOrDefault();
        }
    }
}
