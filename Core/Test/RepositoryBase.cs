using System;
using System.Collections.Generic;
using DataStorage.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class RepositoryBaseTest
    {
        private Repository Repository;
        [TestInitialize]
        public void Setup()
        {
            Repository = new Repository();
        }

        [TestMethod]
        public void MetaFieldsAreSetAtStartup()
        {
            Assert.IsNotNull(Repository.MetaFields);
        }

        [TestMethod]
        public void DataSourcesIsNull()
        {
            Assert.IsNull(Repository.DataSources);
        }

        [TestMethod]
        public void MetaFieldsShouldBeAutoMappedByDefaut()
        {
            foreach (var metaField in Repository.MetaFields.AsList())
            {
                Assert.IsTrue(ClassMap.ShouldAutoMapMembers.Contains(metaField));
            }
        }

        [TestMethod]
        public void DeleteItemThrows()
        {
            Assert.ThrowsException<NotImplementedException>(() => Repository.Delete(new Class()));
        }

        [TestMethod]
        public void DeleteItemIdThrows()
        {
            Assert.ThrowsException<NotImplementedException>(() => Repository.Delete("id"));
        }

        [TestMethod]
        public void DeleteFilterThrows()
        {
            Assert.ThrowsException<NotImplementedException>(() => Repository.Delete<Class>(x => x.Id == "Id"));
        }

        [TestMethod]
        public void GetItemThrows()
        {
            Assert.ThrowsException<NotImplementedException>(() => Repository.Get(new Class()));
        }

        [TestMethod]
        public void GetItemIdThrows()
        {
            Assert.ThrowsException<NotImplementedException>(() => Repository.Get("id"));
        }

        [TestMethod]
        public void FindFilterThrows()
        {
            Assert.ThrowsException<NotImplementedException>(() => Repository.Find<Class>(x => x.Name == "name"));
        }

        [TestMethod]
        public void InsertItemThrows()
        {
            Assert.ThrowsException<NotImplementedException>(() => Repository.Insert(new Class()));
        }

        [TestMethod]
        public void InsertItemsThrows()
        {
            Assert.ThrowsException<NotImplementedException>(() => Repository.Insert(new List<Class> { new Class() }));
        }

        [TestMethod]
        public void ReplaceThrows()
        {
            Assert.ThrowsException<NotImplementedException>(() => Repository.Replace(new Class()));
        }
    }
}