using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataStorage.Core
{
    public interface IRepository
    {
        Task<T> Get<T>(T item);
        Task<T> Get<T>(string itemId);
        Task<List<T>> Get<T>();
        Task<T> Insert<T>(T item);
        Task<List<T>> Insert<T>(List<T> items);
        Task Delete<T>(T item);
        Task Delete<T>(string itemId);
        Task Delete<T>();
        void ValidateProperties();
        Dictionary<Type, string> DataSources { get; set; }
    }
}