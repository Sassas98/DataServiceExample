using Magic;
using Magic.Bond;

namespace DataServiceExample.Models
{
    public class Exam : Trick
    {
        public ulong Id { get; set; }
        public int Vote { get; set; }
        [BondKey(typeof(Iscription))]
        public ulong IscriptionId { get; set; }
        public Scroll<Iscription> Iscription { get; set; }
    }
}
