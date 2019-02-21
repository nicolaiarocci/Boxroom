using System;
using System.Collections.Generic;
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
        public void RenderAsQueryString()
        {
            var aClassInstance = new Class { AProperty = "a property value" };
            var aVariable = DateTime.Now.Date;

            var box = new WebApiBox();

            var result = box.RenderAsQueryString<Class>(x =>
               x.Name == "a name" &&
               x.AProperty == aClassInstance.AProperty &&
               x.UpdatedOn == aVariable
                );

            Assert.AreEqual("?Name=a%20name&AProperty=a%20property%20value&UpdatedOn=Thu,%2014%20Feb%202019%2000:00:00%20GMT", result);
        }
    }
}