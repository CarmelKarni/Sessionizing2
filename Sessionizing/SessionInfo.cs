using System;
using System.Collections.Generic;
using System.Linq;

namespace Sessionizing
{
    internal class SessionInfo : IComparable
    {
        private const int HalfAnHour = 30 * 60;
        public int Length { get; }

        public SessionInfo(int length)
        {
            Length = length;
        }

        private static long GetTimestampAt(List<long> timestamps, int counter)
        {
            if (counter < timestamps.Count)
            {
                return timestamps.ElementAt(counter);
            }

            return long.MaxValue;
        }

        public int CompareTo(object obj)
        {
            if (obj is SessionInfo)
            {
                return Length.CompareTo(((SessionInfo)obj).Length);
            }
            return 1;
        }

        public static IEnumerable<SessionInfo> ParseSessions(List<long> timestamps)
        {
            var sessions = new List<SessionInfo>();
            //var timestamps = visitorGroup.Select(x => x.Timestamp).ToList();
            timestamps.Sort();
            //we'll handle the last on its own
            for (int i = 0; i < timestamps.Count; i++)
            {
                var first = timestamps.ElementAt(i);
                var current = first;
                int counter = i;
                counter++;
                var next = GetTimestampAt(timestamps, counter);
                while (Math.Abs(next - current) < HalfAnHour)
                {
                    current = next;
                    counter++;
                    next = GetTimestampAt(timestamps, counter);
                }
                sessions.Add(new SessionInfo(Convert.ToInt32(current - first)));
                i = --counter;
            }

            return sessions;
        }
    }
}