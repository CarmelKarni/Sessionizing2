using System;

namespace Sessionizing
{
    internal class PageView
    {
        public string Visitor { get; }
        public string Site { get; }
        public string Page { get; }
        public long Timestamp { get; }

        public PageView(string visitor, string site, string page, string timestamp)
        {
            Visitor = visitor;
            Site = site;
            Page = page;
            Timestamp = long.Parse(timestamp);
            //visitor_2040,www.s_8.com,www.s_8.com/page_6,1347897811
        }

        public static PageView ParseLineToPageView(string line)
        {
            var fields = line.Split(',');
            if (fields.Length != 4)
            {
                return null;
            }
            return new PageView(visitor: fields[0], site: fields[1], page: fields[2], timestamp: fields[3]);
        }
    }
}