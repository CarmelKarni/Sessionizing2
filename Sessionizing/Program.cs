using System;
using System.Collections.Generic;
using System.IO;

namespace Sessionizing
{
    class Program
    {
        private static Dictionary<string, SiteInfo> sites;
        private static Dictionary<string, VisitorInfo> visitors;
        private static bool exit = false;
        static void Main(string[] args)
        {
            //collect all files from directory pointed by user
            // argument[0] must be directory
            if (args.Length != 1)
            {
                PrintUsage();
                Console.ReadLine();
                return;
            }
            sites = new Dictionary<string, SiteInfo>();
            visitors = new Dictionary<string, VisitorInfo>();
            foreach (string file in Directory.EnumerateFiles(args[0], "*.csv"))
            {
                string[] lines = File.ReadAllLines(file);
                foreach (string line in lines)
                {
                    PageView pv = PageView.ParseLineToPageView(line);
                    if (pv != null)
                    {
                        AddPvToSites(pv);
                        AddPvToUsers(pv);
                    }
                }
            }

            //Pre-process all data
            foreach (var site in sites)
            {
                site.Value.Process();
            }
            foreach (var visitor in visitors)
            {
                visitor.Value.Process();
            }
            //ask user for queries
            while (!exit)
            {
                DisplayMainMenu();
            }
        }

        private static void DisplayMainMenu()
        {
            Console.WriteLine("Please select query (enter number):\n1. Get sessions data\n2. get # of unique sites visited\n3. exit");
            int option;
            string optionStr = Console.ReadLine();
            if (!int.TryParse(optionStr, out option))
            {
                return;
            }

            switch (option)
            {
                case 1:
                    GetSessionsData();
                    break;
                case 2:
                    GetNumOfUniqueSites();
                    break;
                case 3:
                    exit = true;
                    break;
            }

            Console.WriteLine();
        }

        private static void GetSessionsData()
        {
            Console.WriteLine("\nPlease enter site id:");
            string siteStr = Console.ReadLine();
            siteStr = siteStr ?? string.Empty;
            if (sites.TryGetValue(siteStr, out var site))
            {
                Console.WriteLine($"Num sessions for site {siteStr} = {site.NumSessions} median session length = {site.MedianSessionLength}");
                return;
            }
            Console.WriteLine($"No sessions for site {siteStr}");
        }

        private static void GetNumOfUniqueSites()
        {
            Console.WriteLine("\nPlease enter visitor id:");
            string visitorStr = Console.ReadLine();
            visitorStr = visitorStr ?? string.Empty;
            if (visitors.TryGetValue(visitorStr, out var visitor))
            {
                Console.WriteLine($"Num of unique sites for {visitorStr} = {visitor.UniqueSites}");
                return;
            }
            Console.WriteLine($"Num of unique sites for {visitorStr} = 0");
        }

        private static void AddPvToSites(PageView pv)
        {
            if (!sites.TryGetValue(pv.Site, out SiteInfo site))
            {
                site = new SiteInfo(pv.Site);
                sites[pv.Site] = site;
            }

            site.Add(pv);
        }

        private static void AddPvToUsers(PageView pv)
        {
            if (!visitors.TryGetValue(pv.Visitor, out VisitorInfo visitor))
            {
                visitor = new VisitorInfo(pv.Visitor);
                visitors[pv.Visitor] = visitor;
            }

            visitor.Add(pv);
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Usage: Sessionizing.exe <directory-of-csv>");
        }
    }
}
