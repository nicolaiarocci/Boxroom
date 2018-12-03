using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Boxroom.Database
{
    public interface IRedisBox
    {
        Task<T> Insert<T>(T item, TimeSpan? expiry);
        Task<List<T>> Insert<T>(List<T> items, TimeSpan? expiry);
        Task<T> Replace<T>(T item, TimeSpan? expiry);
        Task<bool> Exists(string itemId);
        Task<bool> Exists<T>(T item);
        IConnectionMultiplexer Multiplexer { get; set; }
    }
}