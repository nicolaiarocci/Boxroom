using System;
using DataStorage.Rest;
using NUnit.Framework;

namespace Test
{
    public class WebApiRestRepositoryTest
    {
        [Test]
        public void RenderReturnsNullWhenFilterIsNull()
        {
            var repository = new WebApiRestRepository();
            Assert.IsNull(repository.Render<Class>(null));
        }

        [Test]
        public void RenderThrowsNotImplmented()
        {
            var repository = new WebApiRestRepository();
            Assert.Throws<NotImplementedException>(() => repository.Render<Class>(x => x.Name == "a name"));
        }
    }
}