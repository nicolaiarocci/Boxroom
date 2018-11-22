using System;
using Boxroom.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class MetaFieldsTest
    {
        private MetaFields MetaFields;

        [TestInitialize]
        public void Setup()
        {
            MetaFields = new MetaFields();
        }

        [TestMethod]
        public void Defaults()
        {
            Assert.AreEqual("Id", MetaFields.Id);
            Assert.AreEqual("LastUpdated", MetaFields.LastUpdated);
        }

        [TestMethod]
        public void AsList()
        {
            var challenge = MetaFields.AsList();
            Assert.IsTrue(challenge.Contains(MetaFields.Id));
            Assert.IsTrue(challenge.Contains(MetaFields.LastUpdated));
        }
    }
}