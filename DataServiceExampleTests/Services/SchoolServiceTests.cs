using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataServiceExample.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magic;

namespace DataServiceExample.Services.Tests
{
    [TestClass()]
    public class SchoolServiceTests
    {
        [TestCleanup()]
        public void CleanUp()
        {
            SpellBook.RestoreOrigin();
        }

        [TestMethod()]
        public void NewInstanceTest()
        {
            var service1 = SchoolService.NewInstance();
            var service2 = SchoolService.NewInstance();
            Assert.IsTrue(service1 == service2);
        }

        [TestMethod()]
        public void AddSchoolTest()
        {
            var service = SchoolService.NewInstance();
            service.AddSchool("high-school-test", "road-test");
            var school = service.GetSchool("high-school-test");
            Assert.IsTrue(school != null);
            Assert.IsTrue(school.Address == "road-test");
        }

        [TestMethod()]
        public void AddNewStudentTest()
        {
            var service = SchoolService.NewInstance();
            Assert.IsFalse(service.AddNewStudent("Name", "Surname", "123456", 20, "high-school-test"));
            service.AddSchool("high-school-test", "road-test");
            service.AddNewStudent("Name", "Surname", "123456", 20, "high-school-test");
            Assert.IsTrue(service.GetSchool("high-school-test")?.Students.Unroll().Count() == 1);
            Assert.IsTrue(service.GetSchool("high-school-test")?.Students.Unroll().First().Name == "Name");
        }

        [TestMethod()]
        public void GiveUpOnStudiesTest()
        {
            var service = SchoolService.NewInstance();
            service.AddSchool("high-school-test", "road-test");
            service.AddNewStudent("Name", "Surname", "123456", 20, "high-school-test");
            Assert.IsTrue(service.GetSchool("high-school-test")?.Students.Unroll().Count() == 1);
            Assert.IsTrue(service.GiveUpOnStudies("123456", "high-school-test"));
            Assert.IsTrue(service.GetSchool("high-school-test")?.Students.Unroll().Count() == 0);
        }

        [TestMethod()]
        public void OpenClassTest()
        {
            var service = SchoolService.NewInstance();
            Assert.IsFalse(service.OpenClass("History", "high-school-test"));
            service.AddSchool("high-school-test", "road-test");
            service.OpenClass("History", "high-school-test");
            Assert.IsTrue(service.GetSchool("high-school-test")?.Classes.Unroll().Count() == 1);
            Assert.IsTrue(service.GetSchool("high-school-test")?.Classes.Unroll().First().Name == "History");
        }

        [TestMethod()]
        public void CloseClassTest()
        {
            var service = SchoolService.NewInstance();
            service.AddSchool("high-school-test", "road-test");
            service.OpenClass("History", "high-school-test");
            Assert.IsTrue(service.GetSchool("high-school-test")?.Classes.Unroll().Count() == 1);
            Assert.IsTrue(service.CloseClass("History", "high-school-test"));
            Assert.IsTrue(service.GetSchool("high-school-test")?.Classes.Unroll().Count() == 0);
        }

        [TestMethod()]
        public void NewIscriptionTest()
        {
            var service = SchoolService.NewInstance();
            service.AddSchool("high-school-test", "road-test");
            service.OpenClass("History", "high-school-test");
            service.AddNewStudent("Name", "Surname", "123456", 20, "high-school-test");
            var student = service.GetStudent("123456", "high-school-test");
            var _class = service.GetClass("History", "high-school-test");
            Assert.IsTrue(service.NewIscription(student, _class));
            Assert.IsTrue(student.Iscriptions.Unroll().Single().Class.Unroll().Name == "History");
            Assert.IsTrue(_class.Iscriptions.Unroll().Single().Student.Unroll().Name == "Name");
        }

        [TestMethod()]
        public void TakeTheExamTest()
        {
            var service = SchoolService.NewInstance();
            service.AddSchool("high-school-test", "road-test");
            service.OpenClass("History", "high-school-test");
            service.AddNewStudent("Name", "Surname", "123456", 20, "high-school-test");
            var student = service.GetStudent("123456", "high-school-test");
            var _class = service.GetClass("History", "high-school-test");
            service.NewIscription(student, _class);
            var inscription = student.Iscriptions.Unroll().Single();
            var test = inscription.Exam.Unroll();
            Assert.IsTrue(inscription.Exam.Unroll() == null);
            service.TakeTheExam(inscription, 31);
            inscription = student.Restore().Iscriptions.Unroll().Single();
            Assert.IsTrue(inscription.Exam.Unroll() != null);
            Assert.IsTrue(inscription.Exam.Unroll().Vote == 31);
        }
    }
}