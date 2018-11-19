using System;
using System.Reflection;
using DataStorage.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class MemberMapTest
    {
        private MemberMap MemberMap;
        private MemberInfo Challenge;

        [TestInitialize]
        public void Setup()
        {
            Challenge = typeof(Class).GetMember("Id") [0];
            MemberMap = new MemberMap(new ClassMap(typeof(Class)), Challenge);
        }

        [TestMethod]
        public void MemberName()
        {
            Assert.AreEqual(Challenge.Name, MemberMap.MemberName);
        }

        [TestMethod]
        public void ClassMap()
        {
            Assert.IsNotNull(MemberMap.ClassMap.GetType());
        }

        [TestMethod]
        public void MemberInfo()
        {
            Assert.IsNotNull(MemberMap.MemberInfo);
            Assert.AreEqual(Challenge.Name, MemberMap.MemberInfo.Name);
        }

        [TestMethod]
        public void MemberType()
        {
            Assert.IsNotNull(MemberMap.MemberType);
            Assert.AreEqual(((PropertyInfo) Challenge).PropertyType, ((PropertyInfo) MemberMap.MemberInfo).PropertyType);
        }

        [TestMethod]
        public void GetMemberInfoTypeThrows()
        {
            Assert.ThrowsException<ArgumentNullException>(() => MemberMap.GetMemberInfoType(null));
            Assert.ThrowsException<NotSupportedException>(() => MemberMap.GetMemberInfoType(typeof(Class)));
        }

        [TestMethod]
        public void GetMemberInfoTypeSupportsPropertyInfo()
        {
            Assert.AreEqual(((PropertyInfo) Challenge).PropertyType, MemberMap.GetMemberInfoType(Challenge));
        }

        [TestMethod]
        public void GetMemberInfoTypeSupportsFieldInfo()
        {
            var challenge = typeof(Class).GetMember("Field") [0];
            Assert.AreEqual(((FieldInfo) challenge).FieldType, MemberMap.GetMemberInfoType(challenge));
        }

        [TestMethod]
        public void GetMemberInfoNameThrows()
        {
            Assert.ThrowsException<ArgumentNullException>(() => MemberMap.GetMemberInfoName(null));
            Assert.ThrowsException<NotSupportedException>(() => MemberMap.GetMemberInfoName(typeof(Class)));
        }

        [TestMethod]
        public void GetMemberInfoNameSupportsPropertyInfo()
        {
            Assert.AreEqual(((PropertyInfo) Challenge).Name, MemberMap.GetMemberInfoName(Challenge));
        }

        [TestMethod]
        public void GetMemberInfoNameSupportsFieldInfo()
        {
            var challenge = typeof(Class).GetMember("Field") [0];
            Assert.AreEqual(((FieldInfo) challenge).Name, MemberMap.GetMemberInfoName(challenge));
        }
    }
}