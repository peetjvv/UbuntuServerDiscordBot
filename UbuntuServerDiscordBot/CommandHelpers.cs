using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UbuntuServerDiscordBot
{
    public static class CommandHelpers
    {
        public static string GetIPv4Address()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            var result = host.AddressList.FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);
            if (result != null)
            {
                return result.ToString();
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public static string GetIPv6Address()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            var result = host.AddressList.FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetworkV6);
            if (result != null)
            {
                return result.ToString();
            }
            throw new Exception("No network adapters with an IPv6 address in the system!");
        }
    }
}
