using System;
using System.Collections.Generic;
using System.Text;

namespace DuckWalk
{
    public class Data
    {
        public string ipAddress { get; set; }
        public bool isPublic { get; set; }
        public int ipVersion { get; set; }
        public bool isWhitelisted { get; set; }
        public int abuseConfidenceScore { get; set; }
        public string countryCode { get; set; }
        public string usageType { get; set; }
        public string isp { get; set; }
        public string domain { get; set; }
        public List<object> hostnames { get; set; }
        public int totalReports { get; set; }
        public int numDistinctUsers { get; set; }
        public DateTime lastReportedAt { get; set; }
    }

    public class Root
    {
        public Data data { get; set; }
    }
}
