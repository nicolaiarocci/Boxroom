using System;
using Boxroom.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class ClassMapTest
    {
        [TestMethod]
        public void RegisterClassMapInstance()
        {
            ClassMap.ShouldAutoMapMembers.Clear();
            Assert.ThrowsException<ArgumentNullException>(() => ClassMap.RegisterClassMap(null));

            var map = new ClassMap(typeof(Class));
            ClassMap.RegisterClassMap(map);
            map = ClassMap.LookupClassMap(typeof(Class));

            Assert.IsNotNull(map);
            Assert.IsTrue(map.ClassType == typeof(Class));
            Assert.IsNull(map.GetMap("Id"));

            // Does not crash; silently ignored. 
            ClassMap.RegisterClassMap(map);
        }

        [TestMethod]
        public void RegisterClassMapiGeneric()
        {
            ClassMap.ShouldAutoMapMembers.Clear();
            ClassMap.RegisterClassMap<Class>();
            var map = ClassMap.LookupClassMap(typeof(Class));

            Assert.IsTrue(map.ClassType == typeof(Class));
            Assert.IsNull(map.GetMap("Id"));
        }

        [TestMethod]
        public void RegisterClassMapiWithInitializer()
        {
            ClassMap.ShouldAutoMapMembers.Clear();
            ClassMap.RegisterClassMap<Class>(cm => cm.AutoMap());
            var map = ClassMap.LookupClassMap(typeof(Class));

            Assert.IsTrue(map.ClassType == typeof(Class));
            Assert.IsNull(map.GetMap("Id"));
        }

        [TestMethod]
        public void AutoMap()
        {
            ClassMap.ShouldAutoMapMembers.Clear();
            ClassMap.RegisterClassMap<Class>();

            var map = ClassMap.LookupClassMap(typeof(Class));

            Assert.IsNull(map.GetMap("Id"));
            Assert.IsNull(map.GetMap("UpdatedOn"));
            Assert.ThrowsException<ArgumentNullException>(() => map.GetMap(null));

            ClassMap.ShouldAutoMapMembers.AddRange(new string[] { "Id", "UpdatedOn" });
            map.AutoMap();

            var memberMap = map.GetMap("Id");
            Assert.IsNotNull(memberMap);
            Assert.AreEqual(memberMap.MemberName, "Id");
            Assert.AreEqual(memberMap.MemberType, typeof(string));
            Assert.AreEqual(memberMap.MemberInfo.Name, "Id");

            memberMap = map.GetMap("UpdatedOn");
            Assert.IsNotNull(memberMap);
            Assert.AreEqual(memberMap.MemberName, "UpdatedOn");
            Assert.AreEqual(memberMap.MemberType, typeof(DateTime));
            Assert.AreEqual(memberMap.MemberInfo.Name, "UpdatedOn");

            memberMap = map.GetMap("Name");
            Assert.IsNull(memberMap);
        }

        [TestMethod]
        public void ManualMap()
        {
            ClassMap.ShouldAutoMapMembers.Clear();
            var genericMap = ClassMap.RegisterClassMap<Class>();

            Assert.IsNull(genericMap.GetMap("Id"));
            Assert.IsNull(genericMap.GetMap("UpdatedOn"));
            Assert.IsNull(genericMap.GetMap("Name"));
            Assert.ThrowsException<ArgumentNullException>(() => genericMap.GetMap(null));

            genericMap.MapMember(x => x.Name, "NameField");

            var memberMap = genericMap.GetMap("NameField");
            Assert.AreEqual(memberMap.MemberName, "Name");
            Assert.AreEqual(memberMap.MemberType, typeof(string));
            Assert.AreEqual(memberMap.MemberInfo.Name, "Name");

            genericMap.MapMember(x => x.UpdatedOn, "UpdatedField");

            memberMap = genericMap.GetMap("UpdatedField");
            Assert.AreEqual(memberMap.MemberName, "UpdatedOn");
            Assert.AreEqual(memberMap.MemberType, typeof(DateTime));
            Assert.AreEqual(memberMap.MemberInfo.Name, "UpdatedOn");

            var nonGenericMap = ClassMap.LookupClassMap(typeof(Class));
            memberMap = genericMap.GetMap("NameField");
            Assert.AreEqual(memberMap.MemberName, "Name");
            Assert.AreEqual(memberMap.MemberType, typeof(string));
            Assert.AreEqual(memberMap.MemberInfo.Name, "Name");
            memberMap = genericMap.GetMap("UpdatedField");
            Assert.AreEqual(memberMap.MemberName, "UpdatedOn");
            Assert.AreEqual(memberMap.MemberType, typeof(DateTime));
            Assert.AreEqual(memberMap.MemberInfo.Name, "UpdatedOn");
        }

        [TestMethod]
        public void LookupClassMapRegistersAndAutoMapsMissingMapClass()
        {
            ClassMap.ShouldAutoMapMembers.Add("Id");
            var map = ClassMap.LookupClassMap(typeof(Class));

            Assert.IsNotNull(map);
            Assert.IsNotNull(map.GetMap("Id"));
        }
    }
}