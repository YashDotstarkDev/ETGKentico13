using System.Collections.Generic;

namespace ETG.WebAPI.Models.SearchTypeAhead
{
    public class All
    {
        public string title { get; set; }
        public string url { get; set; }
    }

    public class Link
    {
        public string title { get; set; }
        public string url { get; set; }
    }

    public class Tours
    {
        public string name { get; set; }
        public List<Link> results { get; set; }
        public List<Link> links { get; set; }
    }


    public class Destinations
    {
        public string name { get; set; }
        public List<Link> results { get; set; }
        public List<Link> links { get; set; }
    }

    public class Inspired
    {
        public string name { get; set; }
        public List<Link> results { get; set; }
        public List<Link> links { get; set; }
    }

    public class Results
    {
        public Tours tours { get; set; }
        public Destinations destinations { get; set; }
        public Inspired inspired { get; set; }
    }

    public class SearchTypeAheadResult
    {
        public All all { get; set; }
        public Results results { get; set; }
    }
}