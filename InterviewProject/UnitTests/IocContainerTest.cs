using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IocContainer;

namespace UnitTests
{
    [TestFixture]
    public class IocContainerTest
    {
        [Test]
        public void TestSingleton()
        {
            IocContainer.IocContainer.ClearRegistry();
            IocContainer.IocContainer.Register<ITeacher, Teacher>(LifeStyleType.Singleton);
            IocContainer.IocContainer.Register<IClassroom, Classroom>();
            Teacher t1 = (Teacher)IocContainer.IocContainer.Resolve(typeof(ITeacher));
            Teacher t2 = (Teacher)IocContainer.IocContainer.Resolve(typeof(ITeacher));
            t1.name = "Billy";
            Assert.AreEqual(t1.name, t2.name);
        }

        [Test]
        public void TestRegister()
        {
            IocContainer.IocContainer.ClearRegistry();
            IocContainer.IocContainer.Register<ITeacher, Teacher>();
            IocContainer.IocContainer.Register<IClassroom, Classroom>();
            var t = IocContainer.IocContainer.Resolve(typeof(ITeacher));
            Assert.IsInstanceOf<Teacher>(t);
        }

        [Test]
        public void TestDependencyInjection()
        {
            IocContainer.IocContainer.ClearRegistry();
            IocContainer.IocContainer.Register<ITeacher, Teacher>();
            IocContainer.IocContainer.Register<IClassroom, Classroom>();
            var t = IocContainer.IocContainer.Resolve(typeof(ITeacher));
            Assert.IsNotNull(((Teacher)t).classroom);
        }

        [Test]
        public void TestTransient()
        {
            IocContainer.IocContainer.ClearRegistry();
            IocContainer.IocContainer.Register<ITeacher, Teacher>();
            IocContainer.IocContainer.Register<IClassroom, Classroom>();
            Teacher t1 = (Teacher)IocContainer.IocContainer.Resolve(typeof(ITeacher));
            Teacher t2 = (Teacher)IocContainer.IocContainer.Resolve(typeof(ITeacher));
            t1.classroom.name = "Room 200";
            t2.classroom.name = "Room 300";
            Assert.AreNotEqual(((Teacher)t1).classroom.name, ((Teacher)t2).classroom.name);
        }
    }

    public interface IClassroom 
    {
        string name { get; set; }
    }

    public class Classroom : IClassroom
    {
        public string name { get; set; }

        public Classroom() { }
    }

    public interface ITeacher
    {
        string name { get; set; }
        int age { get; set; }
        IClassroom classroom { get; set; }
    }

    public class Teacher : ITeacher
    {
        public string name { get; set; }
        public int age { get; set; }
        public IClassroom classroom { get; set; }

        public Teacher(IClassroom c)
        {
            classroom = c;
        }
    }
}
