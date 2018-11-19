using DataStorage.Rest;
using NUnit.Framework;

namespace Test
{
    public class MetaFieldsTest
    {
        [Test]
        public void Defaults()
        {
            var restMetaFields = new MetaFields();

            Assert.AreEqual("ETag", restMetaFields.ETag);
            Assert.AreEqual("DateCreated", restMetaFields.DateCreated);
        }
    }
}