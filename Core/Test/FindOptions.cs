using System;
using Boxroom.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class FindOptionsTest
    {
        private FindOptions<Class> FindOptions;

        [TestInitialize]
        public void Setup()
        {
            FindOptions = new FindOptions<Class>();
        }

        [TestMethod]
        public void Defaults()
        {
            Assert.IsFalse(FindOptions.IfModifiedSince.HasValue);
        }
    }
}