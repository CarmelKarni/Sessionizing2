using System.Collections.Generic;

namespace Sessionizing
{
    internal class VisitorInfo
    {
        public string Visitor { get; }
        public int UniqueSites { get; private set; }
        private readonly HashSet<string> m_sites;

        public VisitorInfo(string visitor)
        {
            Visitor = visitor;
            m_sites = new HashSet<string>();
        }

        public void Add(PageView pv)
        {
            if (!m_sites.Contains(pv.Site))
            {
                m_sites.Add(pv.Site);
            }
        }

        public void Process()
        {
            UniqueSites = m_sites.Count;
        }
    }
}