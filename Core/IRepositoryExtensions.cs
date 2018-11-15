using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataStorage.Core
{
    public static class IRepositoryExtensions
    {
        public static Task<List<T>> Find<T>(this IRepository repository, IFindOptions<T> options = null)
        {
            return repository.Find<T>(null, options);
        }
    }
}