namespace ETG.Web.Models.Common
{
    public class MapItemViewModel : IViewModel
    {
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Url { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
