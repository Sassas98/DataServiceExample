using Magic;
using Magic.Bond;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceExample.Models
{
    public class Student : Artifact
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string SerialKey { get; set; } = string.Empty;
        public int Age { get; set; }
        [BondKey(typeof(School))]
        public ulong SchoolId { get; set; }
        public Scroll<School> School { get; set; }
        public Scroll<IEnumerable<Iscription>> Iscriptions { get; set; }
    }
}
