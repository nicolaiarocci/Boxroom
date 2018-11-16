using System;
using DataStorage.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class IRepositoryExtensionsTest
    {
        private Repository Repository;

        [TestInitialize]
        public void Setup()
        {
            Repository = new Repository();
        }

        [TestMethod]
        public void FindThrows()
        {
            Assert.ThrowsException<NotImplementedException>(() => Repository.Find<Class>(x => x.Name == "name"));
        }
    }
}