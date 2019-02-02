using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UbuntuServerDiscordBot.Entities
{
    public class TopResponse
    {
        public DateTime Timestamp { get; private set; }
        public TimeSpan Uptime { get; private set; }

        public int NumUsers { get; private set; }

        public int NumTotalTasks { get; private set; }
        public int NumRunningTasks { get; private set; }
        public int NumSleepingTasks { get; private set; }
        public int NumStoppedTasks { get; private set; }
        public int NumZombieTasks { get; private set; }

        public double CpuUsedPercentage { get; private set; }
        public double CpuIdlePercentage { get; private set; }

        public double TotalMemoryInKiB { get; private set; }
        public double FreeMemoryInKiB { get; private set; }
        public double UsedMemoryInKiB { get; private set; }

        public List<UbuntuProcess> Processes { get; private set; }
        public int NumProcesses { get { return Processes == null ? -1 : Processes.Count; } }

        public override string ToString()
        {
            return base.ToString(); // TODO?
        }

        public static TopResponse FromString(string topResponseString)
        {
            var lines = topResponseString.SplitOnNewLines();
            var splittedLines = lines.Select(x => x.Split(',').TrimAll()).ToArray();

            var daysString = lines[0].Substring(18, lines[0].IndexOf("days") - 18).Trim();
            var days = int.Parse(daysString);
            var hoursString = splittedLines[0][1].Split(':')[0];
            var hours = int.Parse(hoursString);
            var minutesString = splittedLines[0][1].Split(':')[1];
            var minutes = int.Parse(minutesString);
            var uptime = new TimeSpan(days, hours, minutes, 0);

            var processes = new List<UbuntuProcess>();
            for (int i = 7; i < lines.Length; i++)
            {
                processes.Add(UbuntuProcess.FromTopResponseProcessLine(lines[i]));
            }

            return new TopResponse()
            {
                Timestamp = DateTime.Now.Date.AddTimeSpan(TimeSpan.Parse(lines[0].Substring(6, "12:13:44".Length - 6))),
                Uptime = uptime,

                NumUsers = int.Parse(splittedLines[0][2].Split(' ')[0]),

                NumTotalTasks = int.Parse(splittedLines[1][0].Substring(8, splittedLines[1][0].IndexOf("total") - 8).Trim()),
                NumRunningTasks = int.Parse(splittedLines[1][1].Substring(0, splittedLines[1][1].IndexOf("running")).Trim()),
                NumSleepingTasks = int.Parse(splittedLines[1][2].Substring(0, splittedLines[1][2].IndexOf("sleeping")).Trim()),
                NumStoppedTasks = int.Parse(splittedLines[1][3].Substring(0, splittedLines[1][3].IndexOf("stopped")).Trim()),
                NumZombieTasks = int.Parse(splittedLines[1][4].Substring(0, splittedLines[1][4].IndexOf("zombie")).Trim()),

                CpuUsedPercentage = double.Parse(splittedLines[2][0].Substring(10, splittedLines[2][0].IndexOf("us") - 10).Trim()),
                CpuIdlePercentage = double.Parse(splittedLines[2][3].Substring(0, splittedLines[2][3].IndexOf("id")).Trim()),

                TotalMemoryInKiB = double.Parse(splittedLines[3][0].Substring(11, splittedLines[3][0].IndexOf("total") - 11).Trim()),
                FreeMemoryInKiB = double.Parse(splittedLines[3][1].Substring(0, splittedLines[3][1].IndexOf("free")).Trim()),
                UsedMemoryInKiB = double.Parse(splittedLines[3][2].Substring(0, splittedLines[3][2].IndexOf("used")).Trim()),

                Processes = processes
            };
        }
    }
}
