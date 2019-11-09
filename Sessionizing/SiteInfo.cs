using System.Collections.Generic;
using System.Linq;

namespace Sessionizing
{
    internal class SiteInfo
    {
        public string Site { get; }
        public int NumSessions { get; private set; }
        public float MedianSessionLength { get; private set; }

        //private readonly List<PageView> m_pageViews;
        private readonly Dictionary<string, List<long>> m_viewsByVisitors;
        private readonly List<SessionInfo> m_sessions;

        public SiteInfo(string site)
        {
            Site = site;
            m_viewsByVisitors = new Dictionary<string, List<long>>();
            m_sessions = new List<SessionInfo>();
        }

        public void Add(PageView pv)
        {
            if (!m_viewsByVisitors.TryGetValue(pv.Visitor, out List<long> timestamps))
            {
                timestamps = new List<long>();
                m_viewsByVisitors[pv.Visitor] = timestamps;
            }
            timestamps.Add(pv.Timestamp);
        }

        public void Process()
        {
            foreach (KeyValuePair<string, List<long>> visitorGroup in m_viewsByVisitors)
            {
                m_sessions.AddRange(SessionInfo.ParseSessions(visitorGroup.Value));
            }

            NumSessions = m_sessions.Count;
            m_sessions.Sort();
            ComputeMedianSessionLength();
        }

        private void ComputeMedianSessionLength()
        {
            if (NumSessions % 2 != 0)
            {
                MedianSessionLength = m_sessions.ElementAt(NumSessions / 2).Length;
                return;
            }

            int pos = NumSessions / 2 - 1;
            float smaller = m_sessions.ElementAt(pos).Length;
            float larger = m_sessions.ElementAt(pos + 1).Length;
            MedianSessionLength = (smaller + larger) / 2;
        }
    }
}