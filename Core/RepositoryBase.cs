using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace DataStorage.Core
{
    public abstract class RepositoryBase : IRepository
    {
        public RepositoryBase()
        {
            EnsureMetaFieldsAreMapped();
        }
        public virtual MetaFields MetaFields { get; } = new MetaFields();
        public Dictionary<Type, string> DataSources { get; set; }

        public virtual Task Delete<T>(T item)
        {
            throw new NotImplementedException();
        }

        public virtual Task Delete<T>(string itemId)
        {
            throw new NotImplementedException();
        }

        public virtual Task<T> Delete<T>(Expression<Func<T, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public virtual Task<List<T>> Find<T>(Expression<Func<T, bool>> filter, IFindOptions<T> options = null)
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
        protected(string IdMemberName, object IdMemberValue) GetIdMemberNameAndValue<T>(T item)
        {
            return GetMemberNameAndValue(item, "Id");
        }
        protected(string MemberName, object MemberValue) GetMemberNameAndValue<T>(T item, string name)
        {
            string memberName = null;
            object memberValue = null;

            memberName = GetMemberName<T>(name);
            if (memberName != null)
            {
                memberValue = typeof(T).GetProperty(memberName).GetValue(item);
            }
            return (memberName, memberValue);
        }
        protected string GetMemberName<T>(string name)
        {
            return ClassMap.LookupClassMap(typeof(T)).GetMap(name)?.MemberName;
        }
        private void EnsureMetaFieldsAreMapped()
        {
            foreach (var metaField in MetaFields.AsList())
            {
                if (!ClassMap.ShouldAutoMapMembers.Contains(metaField))
                {
                    ClassMap.ShouldAutoMapMembers.Add(metaField);
                }
            }
        }
    }
}