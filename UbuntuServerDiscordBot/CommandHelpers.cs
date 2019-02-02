using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UbuntuServerDiscordBot.Entities;
using UbuntuServerDiscordBot.Serialization;

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

        public static TopResponse GetTopResponse()
        {
            var topOutputFileUrl = @"C:\Temp\top-output.txt";//"~/top-output.txt";
            //ExecuteCommandAndSaveOutputToFile("top -n 1 -b", topOutputFileUrl);
            var topOutput = "";
            using (var fileStream = File.OpenText(topOutputFileUrl))
            {
                topOutput = fileStream.ReadToEnd();
            }
            return TopResponseDeserializer.Deserialize(topOutput);
        }

        private static void ExecuteCommandAndSaveOutputToFile(string command, string outputFileName)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = "/bin/bash";
            proc.StartInfo.Arguments = $"-c \" {command} > {outputFileName} \"";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
        }
    }
}
