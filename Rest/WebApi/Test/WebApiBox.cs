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
            var c = new Class { AProperty = "a field value" };
            var box = new WebApiBox();

            var result = box.RenderAsQueryString<Class>(
                // a constant value, needs escaping.
                x => x.Name == "a name" &&
                // class property value.
                x.AProperty == c.AProperty &&
                // field (variable) value.
                x.UpdatedOn == updatedOn
                );

            Assert.AreEqual("?name=a%20name&aproperty=a%20field%20value&updatedon=Thu,%2014%20Feb%202019%2000:00:00%20GMT", result);
        }
    }
}