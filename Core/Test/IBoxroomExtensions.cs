using System;
using Boxroom.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class IBoxroomExtensionsTest
    {
        private Boxroom Box;

        [TestInitialize]
        public void Setup()
        {
            Box = new Boxroom();
        }

        [TestMethod]
        public void FindThrows()
        {
            Assert.ThrowsException<NotImplementedException>(() => Box.Find<Class>(x => x.Name == "name"));
        }
    }
}