namespace Devotion.Web.Base.Models
{
    public class BaseIdentity
    {
        public string NameIdentifier { get; set; }

        public string Name { get; set; }

        public int UserId { get; set; } = 0;
    }
}