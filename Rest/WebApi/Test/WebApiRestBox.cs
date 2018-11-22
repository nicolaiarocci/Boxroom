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
            var box = new WebApiRestBox();
            Assert.IsNull(box.Render<Class>(null));
        }

        [Test]
        public void RenderThrowsNotImplmented()
        {
            var box = new WebApiRestBox();
            Assert.Throws<NotImplementedException>(() => box.Render<Class>(x => x.Name == "a name"));
        }
    }
}