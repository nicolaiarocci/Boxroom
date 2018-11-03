using System;
using System.Reflection;

namespace DataStorage.Core
{
    public class MemberMap
    {
        private readonly ClassMap _classMap;
        private readonly MemberInfo _memberInfo;
        private readonly Type _memberType;
        private readonly string _memberName;

        public MemberMap(ClassMap classMap, MemberInfo memberInfo)
        {
            _classMap = classMap;
            _memberInfo = memberInfo;
            _memberType = GetMemberInfoType(memberInfo);
            _memberName = GetMemberInfoName(memberInfo);

        }
        public string MemberName
        {
            get { return _memberName; }
        }
        public ClassMap ClassMap
        {
            get { return _classMap; }
        }
        public MemberInfo MemberInfo
        {
            get { return _memberInfo; }
        }
        public Type MemberType
        {
            get { return _memberType; }
        }
        public static Type GetMemberInfoType(MemberInfo memberInfo)
        {
            if (memberInfo == null)
            {
                throw new ArgumentNullException(nameof(memberInfo));
            }

            if (memberInfo is FieldInfo)
            {
                return ((FieldInfo)memberInfo).FieldType;
            }
            else if (memberInfo is PropertyInfo)
            {
                return ((PropertyInfo)memberInfo).PropertyType;
            }

            throw new NotSupportedException("Only field and properties are supported at this time.");
        }
        public static string GetMemberInfoName(MemberInfo memberInfo)
        {
            if (memberInfo == null)
            {
                throw new ArgumentNullException(nameof(memberInfo));
            }

            if (memberInfo is FieldInfo || memberInfo is PropertyInfo)
            {
                return memberInfo.Name;
            }
            throw new NotSupportedException("Only field and properties are supported at this time.");
        }

    }
}