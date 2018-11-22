using System;
using System.Globalization;
using Boxroom.Rest;
using NUnit.Framework;

namespace Test
{
    public class EnsureHeaderTest
    {
        private Boxroom Box;

        [SetUp]
        public void Setup()
        {
            Box = new Boxroom();
        }

        [Test]
        public void IfModifiedSinceHeaderNotAddedWithNullArguments()
        {
            EnsureHeader.IfModifiedSince<Class>(Box, null);
            Assert.IsFalse(Box.Headers.ContainsKey("If-Modified-Since"));

            EnsureHeader.IfModifiedSince<Class>(Box, new RestFindOptions<Class> { IfModifiedSince = null });
            Assert.IsFalse(Box.Headers.ContainsKey("If-Modified-Since"));
        }

        [Test]
        public void IfModifiedSinceHeaderAddedWithValidArgument()
        {

            var challenge = DateTime.Now;
            Box.Headers = null;

            EnsureHeader.IfModifiedSince<Class>(Box, new RestFindOptions<Class> { IfModifiedSince = challenge });

            Assert.IsTrue(Box.Headers.ContainsKey("If-Modified-Since"));

            var ifModifiedSince = DateTime.ParseExact(Box.Headers["If-Modified-Since"],
                CultureInfo.CurrentCulture.DateTimeFormat.RFC1123Pattern, CultureInfo.CurrentCulture);

            Assert.AreEqual(challenge.ToLongDateString(), ifModifiedSince.ToLongDateString());
        }

        [Test]
        public void IfNoneMatchHeaderNotAddedWithNullArguments()
        {
            EnsureHeader.IfNoneMatch<Class>(Box, null);
            Assert.IsFalse(Box.Headers.ContainsKey("If-None-Match"));

            EnsureHeader.IfNoneMatch<Class>(Box, new RestFindOptions<Class> { ETag = null });
            Assert.IsFalse(Box.Headers.ContainsKey("If-None-Match"));
        }

        [Test]
        public void IfNoneMatchHeaderAddedWithValidArgument()
        {

            Box.Headers = null;
            EnsureHeader.IfNoneMatch<Class>(Box, new RestFindOptions<Class> { ETag = "etag" });

            Assert.IsTrue(Box.Headers.ContainsKey("If-None-Match"));
            Assert.AreEqual("etag", Box.Headers["If-None-Match"]);
        }
    }
}