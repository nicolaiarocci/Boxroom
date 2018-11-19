using System.Collections.Generic;
using DataStorage.Core;

namespace DataStorage.Rest
{
    public static class EnsureHeader
    {
        public static void IfModifiedSince<T>(this IRestRepository repository, IFindOptions<T> options)
        {
            if (options == null)
            {
                return;
            }
            if (!options.IfModifiedSince.HasValue)
            {
                return;
            }
            repository.Headers.Add("If-Modified-Since", options.IfModifiedSince.Value.ToString("r"));
        }
        public static void IfNoneMatch<T>(this IRestRepository repository, RestFindOptions<T> options)
        {
            if (options == null)
            {
                return;
            }
            if (options.ETag == null)
            {
                return;
            }
            repository.Headers.Add("If-None-Match", options.ETag);
        }
    }
}