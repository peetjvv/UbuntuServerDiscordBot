using System;
using System.Collections.Generic;
using System.Text;

namespace UbuntuServerDiscordBot
{
    public static class Extensions
    {
        public static string ReplaceIfAtStart(this string _self, string oldValue, string replacement)
        {
            if (_self.StartsWith(oldValue))
            {
                return replacement + _self.Substring(oldValue.Length);
            }
            return _self;
        }
    }
}
