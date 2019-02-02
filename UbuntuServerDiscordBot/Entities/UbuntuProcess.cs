using System;
using System.Collections.Generic;
using System.Text;
using UbuntuServerDiscordBot.Enums;

namespace UbuntuServerDiscordBot.Entities
{
    public class UbuntuProcess
    {
        public uint PID { get; private set; }

        public string User { get; private set; }

        public string Priority { get; private set; } // PR
        public int NI { get; private set; } // ???
        public int VirtualMemorySize { get; private set; } // VIRT
        public int ResidentMemorySize { get; private set; } // RES
        public int SharedMemorySize { get; private set; } // SHR
        public UbuntuProcessStatus Status { get; private set; } // S

        public double CpuPercentage { get; private set; }
        public double MemoryPercentage { get; private set; }

        public TimeSpan CpuTime { get; private set; }

        public string Command { get; private set; }

        public static UbuntuProcess FromTopResponseProcessLine(string processLineInTopResponse)
        {
            var splitLine = processLineInTopResponse.Split(' ', StringSplitOptions.RemoveEmptyEntries).TrimAll();
            return new UbuntuProcess()
            {
                PID = uint.Parse(splitLine[0]),
                User = splitLine[1],
                Priority = splitLine[2],
                NI = int.Parse(splitLine[3]),
                VirtualMemorySize = int.Parse(splitLine[4]),
                ResidentMemorySize = int.Parse(splitLine[5]),
                SharedMemorySize = int.Parse(splitLine[6]),
                Status = (UbuntuProcessStatus)char.Parse(splitLine[7]),
                CpuPercentage = double.Parse(splitLine[8]),
                MemoryPercentage = double.Parse(splitLine[9]),
                CpuTime = TimeSpan.FromSeconds(int.Parse(splitLine[10].Split(':')[0]) * 60 + double.Parse(splitLine[10].Split(':')[1])),
                Command = splitLine[11]
            };
        }
    }
}
