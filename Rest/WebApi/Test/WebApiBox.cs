using System;
using Boxroom.Rest;
using NUnit.Framework;

namespace Test
{
    public class WebApiRestBoxTest
    {
        [Test]
        public void RenderReturnsNullWhenFilterIsNull()
        {
            var box = new WebApiBox();
            Assert.IsNull(box.RenderAsQueryString<Class>(null));
        }

        [Test]
        public void RenderThrowsNotImplmented()
        {
            var updatedOn = DateTime.Now.Date;
            var c = new Class { AProperty = "a property value" };
            var box = new WebApiBox();

            var result = box.RenderAsQueryString<Class>(
                // a constant value, needs escaping.
                x => x.Name == "a name" &&
                // class property value.
                x.AProperty == c.AProperty &&
                // field (variable) value.
                x.UpdatedOn == updatedOn
                );

            Assert.AreEqual("?Name=a%20name&AProperty=a%20property%20value&UpdatedOn=Thu,%2014%20Feb%202019%2000:00:00%20GMT", result);
        }
    }
}