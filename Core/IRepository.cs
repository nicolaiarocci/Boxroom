using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataStorage.Core
{
    public interface IRepository
    {
        Task<List<T>> Get<T>();
        Task<T> Get<T>(string itemId);
        Task<T> Get<T>(T item);
        Task<T> Insert<T>(T item);
        Task<List<T>> Insert<T>(List<T> items);
        Task Delete<T>(string itemId);
        Task Delete<T>(T item);
        Task<T> Replace<T>(T item);
        void ValidateProperties();
        Dictionary<Type, string> DataSources { get; set; }
    }
}