using Magic;

namespace DataServiceExample.Models
{
    public class School : Trick
    {
        public ulong Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public Scroll<IEnumerable<Class>> Classes { get; set; }
        public Scroll<IEnumerable<Student>> Students { get; set; }
    }
}
