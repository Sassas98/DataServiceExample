using Magic;
using Magic.Bond;

namespace DataServiceExample.Models
{
    public class Class : Trick
    {
        public ulong Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [BondKey(typeof(School))]
        public ulong SchoolId { get; set; }
        public Scroll<School> School { get; set; }
        public Scroll<IEnumerable<Iscription>> Iscriptions { get; set; }
    }
}
