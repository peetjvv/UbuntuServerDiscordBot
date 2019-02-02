using System;
using System.Collections.Generic;
using System.Linq;
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

        public static string[] SplitOnNewLines(this string _self)
        {
            return _self.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        }

        public static string[] TrimAll(this string[] _self)
        {
            return _self.Select(x => x.Trim()).ToArray();
        }

        public static string Substring(this string _self, string substringStart)
        {
            if (_self.Contains(substringStart))
            {
                return _self.Substring(_self.IndexOf(substringStart));
            }

            return null;
        }

        public static DateTime AddTimeSpan(this DateTime _self, TimeSpan ts)
        {
            return _self.AddTicks(ts.Ticks);
        }
    }
}
