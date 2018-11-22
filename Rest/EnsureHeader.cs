using System.Collections.Generic;
using Boxroom.Core;

namespace Boxroom.Rest
{
    public static class EnsureHeader
    {
        public static void IfModifiedSince<T>(this IRestBox box, IFindOptions<T> options)
        {
            if (options == null)
            {
                return;
            }
            if (!options.IfModifiedSince.HasValue)
            {
                return;
            }
            EnsureHeaders(box);

            box.Headers.Add("If-Modified-Since", options.IfModifiedSince.Value.ToString("r"));
        }
        public static void IfNoneMatch<T>(this IRestBox box, RestFindOptions<T> options)
        {
            if (options == null)
            {
                return;
            }
            if (options.ETag == null)
            {
                return;
            }
            EnsureHeaders(box);

            box.Headers.Add("If-None-Match", options.ETag);
        }
        private static void EnsureHeaders(this IRestBox box)
        {
            if (box.Headers == null)
            {
                box.Headers = new Dictionary<string, string>();
            }
        }
    }
}