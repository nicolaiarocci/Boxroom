using System;
using System.Collections.Generic;
using Boxroom.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class BoxroomBaseTest
    {
        private Boxroom Box;
        [TestInitialize]
        public void Setup()
        {
            Box = new Boxroom();
        }

        [TestMethod]
        public void MetaFieldsAreSetAtStartup()
        {
            Assert.IsNotNull(Box.MetaFields);
        }

        [TestMethod]
        public void DataSourcesIsNull()
        {
            Assert.IsNull(Box.DataSources);
        }

        [TestMethod]
        public void MetaFieldsShouldBeAutoMappedByDefaut()
        {
            foreach (var metaField in Box.MetaFields.AsList())
            {
                Assert.IsTrue(ClassMap.ShouldAutoMapMembers.Contains(metaField));
            }
        }

        [TestMethod]
        public void DeleteItemThrows()
        {
            Assert.ThrowsException<NotImplementedException>(() => Box.Delete(new Class()));
        }

        [TestMethod]
        public void DeleteItemIdThrows()
        {
            Assert.ThrowsException<NotImplementedException>(() => Box.Delete("id"));
        }

        [TestMethod]
        public void DeleteFilterThrows()
        {
            Assert.ThrowsException<NotImplementedException>(() => Box.Delete<Class>(x => x.Id == "Id"));
        }

        [TestMethod]
        public void GetItemThrows()
        {
            Assert.ThrowsException<NotImplementedException>(() => Box.Get(new Class()));
        }

        [TestMethod]
        public void GetItemIdThrows()
        {
            Assert.ThrowsException<NotImplementedException>(() => Box.Get("id"));
        }

        [TestMethod]
        public void FindFilterThrows()
        {
            Assert.ThrowsException<NotImplementedException>(() => Box.Find<Class>(x => x.Name == "name"));
        }

        [TestMethod]
        public void InsertItemThrows()
        {
            Assert.ThrowsException<NotImplementedException>(() => Box.Insert(new Class()));
        }

        [TestMethod]
        public void InsertItemsThrows()
        {
            Assert.ThrowsException<NotImplementedException>(() => Box.Insert(new List<Class> { new Class() }));
        }

        [TestMethod]
        public void ReplaceThrows()
        {
            Assert.ThrowsException<NotImplementedException>(() => Box.Replace(new Class()));
        }
    }
}