using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace DataStorage.Core
{
    public abstract class BaseRepository : IRepository
    {
        public Dictionary<Type, string> DataSources { get; set; }

        public virtual Task Delete<T>(T item)
        {
            throw new NotImplementedException();
        }

        public virtual Task Delete<T>(string itemId)
        {
            throw new NotImplementedException();
        }

        public virtual Task<T> Get<T>(T item)
        {
            throw new NotImplementedException();
        }

        public virtual Task<T> Get<T>(string itemId)
        {
            throw new NotImplementedException();
        }

        public virtual Task<List<T>> Get<T>()
        {
            throw new NotImplementedException();
        }

        public virtual Task<T> Insert<T>(T item)
        {
            throw new NotImplementedException();
        }

        public virtual Task<List<T>> Insert<T>(List<T> items)
        {
            throw new NotImplementedException();
        }

        public virtual Task<T> Replace<T>(T item)
        {
            throw new NotImplementedException();
        }

        public abstract void ValidateProperties();
        protected (string IdMemberName, object IdMemberValue) GetIdMemberNameAndValue<T>(T item)
        {
            string idMemberName = null;
            object idMemberValue = null;

            idMemberName = ClassMap.LookupClassMap(typeof(T)).IdMemberMap?.MemberName;
            if (idMemberName != null)
            {
                idMemberValue = typeof(T).GetProperty(idMemberName).GetValue(item);
            }
            return (idMemberName, idMemberValue);
        }
    }
}