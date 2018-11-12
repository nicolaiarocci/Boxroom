using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace DataStorage.Core
{
    public abstract class RepositoryBase : IRepository
    {
        public RepositoryBase()
        {
            if (!ClassMap.ShouldAutoMapMembers.Contains(MetaFields.Id))
            {
                ClassMap.ShouldAutoMapMembers.Add(MetaFields.Id);
            }
        }
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
            return GetMemberNameAndValue(item, "Id");
        }
        protected (string MemberName, object MemberValue) GetMemberNameAndValue<T>(T item, string name)
        {
            string memberName = null;
            object memberValue = null;

            memberName = ClassMap.LookupClassMap(typeof(T)).GetMap(name)?.MemberName;
            if (memberName != null)
            {
                memberValue = typeof(T).GetProperty(memberName).GetValue(item);
            }
            return (memberName, memberValue);
        }
    }
}