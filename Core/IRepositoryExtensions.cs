using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataStorage.Core
{
    public static class IRepositoryExtensions
    {
        public static Task<List<T>> Find<T>(this IRepository repository, FindOptions<T> options = null)
        {
            return repository.Find<T>(_ => true, options);
        }
    }
}