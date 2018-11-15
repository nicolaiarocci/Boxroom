using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataStorage.Core
{
    public interface IRepository
    {
        Task<T> Get<T>(string itemId);
        Task<T> Get<T>(T item);
        Task<List<T>> Find<T>(Expression<Func<T, bool>> filter, IFindOptions<T> options = null);
        Task<T> Insert<T>(T item);
        Task<List<T>> Insert<T>(List<T> items);
        Task Delete<T>(string itemId);
        Task Delete<T>(T item);
        Task<T> Replace<T>(T item);
        Task<T> Delete<T>(Expression<Func<T, bool>> filter);
        void ValidateProperties();
        Dictionary<Type, string> DataSources { get; set; }
        MetaFields MetaFields { get; }
    }
}