using System;
using System.Collections.Generic;
using System.Text;

namespace UbuntuServerDiscordBot.Enums
{
    public enum UbuntuProcessStatus
    {
        UninterruptibleSleep = 'D',
        Running = 'R',
        Sleeping = 'S',
        StoppedByJobControlSignal = 'T',
        StoppedByDebuggerDuringTrace = 't',
        Zombie = 'Z'
    }
}
