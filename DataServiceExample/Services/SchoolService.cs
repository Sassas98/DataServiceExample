using DataServiceExample.Models;
using Magic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceExample.Services
{
    public class SchoolService
    {
        private static object Lock = new object();
        private static SchoolService singleton = null!;

        private SchoolService()
        {
            if(!SpellBook.IsAlreadytInitialized)
                SpellBook.InitiationToArcane("Example", true, false, true);
            SpellBook.BindMysticalBond<School, Class>();
            SpellBook.BindMysticalBond<School, Student>();
            SpellBook.BindMysticalBond<Class, Iscription>();
            SpellBook.BindMysticalBond<Student, Iscription>();
            SpellBook.BindTwinBond<Iscription, Exam>();
            SpellBook.AddBookIndex<School>(s => s.Name);
            SpellBook.AddBookIndex<Student>(s => s.SerialKey + "_" + s.SchoolId);
            SpellBook.AddBookIndex<Class>(s => s.Name + "_" + s.SchoolId);
            SpellBook.AddBookIndex<Iscription>(s => s.StudentId + "_" + s.ClassId);
        }

        public static SchoolService NewInstance()
        {
            lock (Lock) {
                if(singleton == null)
                    singleton = new SchoolService();
                return singleton;
            }
        }

        public bool AddSchool(string name, string address)
        {
            if(SpellBook.GetOne<School>(x => x.Name == name) != null)
                return false;
            SpellBook.SaveOne(new School{Name = name, Address = address});
            return true;
        }

        public School? GetSchool(string name)
        {
            return SpellBook.GetOne<School>(x => x.Name == name);
        }

        public bool AddNewStudent(string name, string surname, string serialKey, int age, string nameOfSchool)
        {
            var school = SpellBook.GetOne<School>(x => x.Name == nameOfSchool);
            if (school == null || school.Students.Unroll().Any(x => x.SerialKey == serialKey))
                return false;
            new Student
            {
                Name = name,
                Surname = surname,
                SerialKey = serialKey,
                Age = age,
                SchoolId = school.Id
            }.Forge();
            return true;
        }

        public bool GiveUpOnStudies(string serialKey, string nameOfSchool)
        {
            var school = SpellBook.GetOne<School>(x => x.Name == nameOfSchool);
            if (school == null)
                return false;
            var student = school.Students.Unroll().FirstOrDefault(x => x.SerialKey == serialKey);
            if (student == null)
                return false;
            SpellBook.DestroyMany(student.Iscriptions.Unroll());
            student.Break();
            return true;
        }

        public Student? GetStudent(string serialKey, string nameOfSchool)
        {
            return SpellBook.GetMany<Student>(x => x.SerialKey == serialKey)
                            .Where(x => x.School.Unroll().Name == nameOfSchool)
                            .FirstOrDefault();
        }

        public bool OpenClass(string name, string nameOfSchool)
        {
            var school = SpellBook.GetOne<School>(x => x.Name == nameOfSchool);
            if (school == null || school.Classes.Unroll().Any(x => x.Name == name))
                return false;
            SpellBook.SaveOne(new Class
            {
                Name = name,
                SchoolId = school.Id
            });
            return true;
        }

        public bool CloseClass(string name, string nameOfSchool)
        {
            var school = SpellBook.GetOne<School>(x => x.Name == nameOfSchool);
            if (school == null)
                return false;
            var _class = school.Classes.Unroll().FirstOrDefault(x => x.Name == name);
            if (_class == null || _class.Iscriptions.Unroll().Any())
                return false;
            SpellBook.DestroyOne(_class);
            return true;
        }

        public Class? GetClass(string name, string nameOfSchool)
        {
            return SpellBook.GetMany<Class>(x => x.Name == name)
                            .Where(x => x.School.Unroll().Name == nameOfSchool)
                            .FirstOrDefault();
        }

        public bool NewIscription(Student student, Class _class)
        {
            if (student.Iscriptions.Unroll().Any(x => x.ClassId == _class.Id))
                return false;
            SpellBook.SaveOne(new Iscription
            {
                StudentId = student.Id,
                ClassId = _class.Id,
            });
            return true;
        }

        public bool TakeTheExam(Iscription iscription, int vote)
        {
            if (iscription.Exam.Unroll() != null)
                return false;
            if(vote < 18 || vote > 31)
                return false;
            SpellBook.SaveOne(new Exam
            {
                Vote = vote,
                IscriptionId = iscription.Id,
            });
            return true;
        }
    }
}
