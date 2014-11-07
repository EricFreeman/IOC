using NUnit.Framework;
using IocContainer;

namespace UnitTests
{
    [TestFixture]
    public class IocContainerTest
    {
        [Test]
        public void TestSingleton()
        {
            Ioc.ClearRegistry();
            Ioc.Register<ITeacher, Teacher>(LifeStyleType.Singleton);
            Ioc.Register<IClassroom, Classroom>();
            var t1 = Ioc.Resolve<ITeacher>();
            var t2 = Ioc.Resolve<ITeacher>();
            t1.name = "Billy";
            Assert.AreEqual(t1.name, t2.name);
        }

        [Test]
        public void TestRegister()
        {
            Ioc.ClearRegistry();
            Ioc.Register<ITeacher, Teacher>();
            Ioc.Register<IClassroom, Classroom>();
            var t = Ioc.Resolve<ITeacher>();
            Assert.IsInstanceOf<Teacher>(t);
        }

        [Test]
        public void TestDependencyInjection()
        {
            Ioc.ClearRegistry();
            Ioc.Register<ITeacher, Teacher>();
            Ioc.Register<IClassroom, Classroom>();
            var t = Ioc.Resolve<ITeacher>();
            Assert.IsNotNull(((Teacher)t).classroom);
        }

        [Test]
        public void TestTransient()
        {
            Ioc.ClearRegistry();
            Ioc.Register<ITeacher, Teacher>();
            Ioc.Register<IClassroom, Classroom>();
            var t1 = Ioc.Resolve<ITeacher>();
            var t2 = Ioc.Resolve<ITeacher>();
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