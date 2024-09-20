using Magic;
using Magic.Bond;

namespace DataServiceExample.Models
{
    public class Iscription : Trick
    {
        public ulong Id { get; set; }
        [BondKey(typeof(Student))]
        public ulong StudentId { get; set; }
        public Scroll<Student> Student { get; set; } = new Scroll<Student>();
        [BondKey(typeof(Class))]
        public ulong ClassId { get; set; }
        public Scroll<Class> Class { get; set; } = new Scroll<Class>();
        [BondKey(typeof(Exam))]
        public ulong? ExamId { get; set; }
        public Scroll<Exam> Exam { get; set; } = new Scroll<Exam>();
    }
}
