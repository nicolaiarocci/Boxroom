using System;
using System.Globalization;
using DataStorage.Rest;
using NUnit.Framework;

namespace Test
{
    public class EnsureHeaderTest
    {
        private Repository Repository;

        [SetUp]
        public void Setup()
        {
            Repository = new Repository();
        }

        [Test]
        public void IfModifiedSinceHeaderNotAddedWithNullArguments()
        {
            EnsureHeader.IfModifiedSince<Class>(Repository, null);
            Assert.IsFalse(Repository.Headers.ContainsKey("If-Modified-Since"));

            EnsureHeader.IfModifiedSince<Class>(Repository, new RestFindOptions<Class> { IfModifiedSince = null });
            Assert.IsFalse(Repository.Headers.ContainsKey("If-Modified-Since"));
        }

        [Test]
        public void IfModifiedSinceHeaderAddedWithValidArgument()
        {

            var challenge = DateTime.Now;
            Repository.Headers = null;

            EnsureHeader.IfModifiedSince<Class>(Repository, new RestFindOptions<Class> { IfModifiedSince = challenge });

            Assert.IsTrue(Repository.Headers.ContainsKey("If-Modified-Since"));

            var ifModifiedSince = DateTime.ParseExact(Repository.Headers["If-Modified-Since"],
                CultureInfo.CurrentCulture.DateTimeFormat.RFC1123Pattern, CultureInfo.CurrentCulture);

            Assert.AreEqual(challenge.ToLongDateString(), ifModifiedSince.ToLongDateString());
        }

        [Test]
        public void IfNoneMatchHeaderNotAddedWithNullArguments()
        {
            EnsureHeader.IfNoneMatch<Class>(Repository, null);
            Assert.IsFalse(Repository.Headers.ContainsKey("If-None-Match"));

            EnsureHeader.IfNoneMatch<Class>(Repository, new RestFindOptions<Class> { ETag = null });
            Assert.IsFalse(Repository.Headers.ContainsKey("If-None-Match"));
        }

        [Test]
        public void IfNoneMatchHeaderAddedWithValidArgument()
        {

            Repository.Headers = null;
            EnsureHeader.IfNoneMatch<Class>(Repository, new RestFindOptions<Class> { ETag = "etag" });

            Assert.IsTrue(Repository.Headers.ContainsKey("If-None-Match"));
            Assert.AreEqual("etag", Repository.Headers["If-None-Match"]);
        }
    }
}