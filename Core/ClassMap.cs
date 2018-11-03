using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DataStorage.Core
{
    public class ClassMap
    {
        private readonly Type _classType;
        private readonly static Dictionary<Type, ClassMap> classMaps = new Dictionary<Type, ClassMap>();
        protected Dictionary<string, MemberMap> memberMaps = new Dictionary<string, MemberMap>();
        protected const string IdName = "Id";

        public ClassMap(Type classType)
        {
            _classType = classType;
        }
        public static ClassMap<TClass> RegisterClassMap<TClass>()
        {
            return RegisterClassMap<TClass>(cm => { cm.AutoMap(); });
        }
        public static ClassMap<TClass> RegisterClassMap<TClass>(Action<ClassMap<TClass>> classMapInitializer)
        {
            var classMap = new ClassMap<TClass>(classMapInitializer);
            RegisterClassMap(classMap);
            return classMap;
        }
        public static void RegisterClassMap(ClassMap classMap)
        {
            if (classMap == null)
            {
                throw new ArgumentNullException(nameof(classMap));
            }
            classMaps.Add(classMap.ClassType, classMap);
        }
        protected MemberMap MapMember(MemberInfo memberInfo)
        {
            if (memberInfo == null)
            {
                throw new ArgumentNullException(nameof(memberInfo));
            }
            if (!(memberInfo is FieldInfo) && !(memberInfo is PropertyInfo))
            {
                throw new ArgumentException("MemberInfo must be either a FieldInfo or a PropertyInfo.", nameof(memberInfo));
            }
            EnsureMemberInfoIsForThisClass(memberInfo);

            return new MemberMap(this, memberInfo);
        }

        private void EnsureMemberInfoIsForThisClass(MemberInfo memberInfo)
        {
            if (memberInfo.DeclaringType != _classType)
            {
                throw new ArgumentOutOfRangeException(
                    "memberInfo",
                    $"The memberInfo argument must be for class {_classType.Name}, but was for class {memberInfo.DeclaringType.Name}."
                    );
            }
        }

        protected void SetIdMember(MemberMap memberMap)
        {
            if (memberMap == null)
            {
                throw new ArgumentNullException(nameof(memberMap));
            }

            EnsureMemberMapIsForThisClass(memberMap);
            SetMemberInMaps(IdName, memberMap);

        }
        protected void SetMemberInMaps(string key, MemberMap memberMap)
        {
            if (!memberMaps.ContainsKey(key))
            {
                memberMaps.Add(key, memberMap);
            }
            else
            {
                memberMaps[key] = memberMap;
            }
        }
        private void EnsureMemberMapIsForThisClass(MemberMap memberMap)
        {
            if (memberMap.ClassMap != this)
            {
                throw new ArgumentOutOfRangeException(
                    "memberMap",
                    $"The memberMap argument must be for class {_classType.Name}, but was for class {memberMap.ClassMap.ClassType.Name}."
                    );
            }
        }
        public static ClassMap LookupClassMap(Type classType)
        {
            if (classType == null)
            {
                throw new ArgumentNullException(nameof(classType));
            }

            ClassMap classMap;
            if (classMaps.TryGetValue(classType, out classMap))
            {
                return classMap;
            }

            if (!classMaps.TryGetValue(classType, out classMap))
            {
                var classMapDefinition = typeof(ClassMap<>);
                var classMapType = classMapDefinition.MakeGenericType(classType);
                classMap = (ClassMap)Activator.CreateInstance(classMapType);
                classMap.AutoMap();
                RegisterClassMap(classMap);
            }
            return classMap;
        }
        public virtual void AutoMap()
        {
            AutoMapIdMember();
        }
        protected void AutoMapIdMember()
        {
            var idMemberInfo = GetMemberInfoByName(IdName);
            if (idMemberInfo == null) return;
            MapIdMember(idMemberInfo);
        }
        private MemberMap MapIdMember(MemberInfo memberInfo)
        {
            var map = MapMember(memberInfo);
            SetIdMember(map);
            return map;
        }
        protected MemberInfo GetMemberInfoByName(string expectedMemberName)
        {
            return _classType.GetTypeInfo().GetMember(expectedMemberName).FirstOrDefault();
        }
        public MemberMap IdMemberMap
        {
            get
            {
                MemberMap idMemberMap;
                if (memberMaps.TryGetValue(IdName, out idMemberMap))
                {
                    return idMemberMap;
                }
                return null;
            }
        }
        public Type ClassType
        {
            get { return _classType; }
        }
    }
    public class ClassMap<TClass> : ClassMap
    {
        public ClassMap() : base(typeof(TClass))
        {
        }

        public ClassMap(Action<ClassMap<TClass>> classMapInitializer) : base(typeof(TClass))
        {
            classMapInitializer(this);
        }

        public MemberMap MapIdMember<TMember>(Expression<Func<TClass, TMember>> memberLambda)
        {
            var map = MapMember(memberLambda);
            SetIdMember(map);
            return map;

        }
        private MemberMap MapMember<TMember>(Expression<Func<TClass, TMember>> propertyLambda)
        {
            var memberInfo = GetMemberInfoFromLambda(propertyLambda);
            return MapMember(memberInfo);
        }
        public MemberMap MapMember<TMember>(string key, Expression<Func<TClass, TMember>> propertyLambda)
        {
            var map = MapMember(propertyLambda);
            SetMemberInMaps(key, map);
            return map;
        }
        private static MemberInfo GetMemberInfoFromLambda<TMember>(Expression<Func<TClass, TMember>> memberLambda)
        {
            var body = memberLambda.Body;
            MemberExpression memberExpression;
            switch (body.NodeType)
            {
                case ExpressionType.MemberAccess:
                    memberExpression = (MemberExpression)body;
                    break;
                case ExpressionType.Convert:
                    var convertExpression = (UnaryExpression)body;
                    memberExpression = (MemberExpression)convertExpression.Operand;
                    break;
                default:
                    throw new ArgumentException("Invalid lambda expression");
            }
            var memberInfo = memberExpression.Member;
            if (memberInfo is PropertyInfo)
            {
                if (memberInfo.DeclaringType.GetTypeInfo().IsInterface)
                {
                    memberInfo = FindPropertyImplementation((PropertyInfo)memberInfo, typeof(TClass));
                }
            }
            else if (!(memberInfo is FieldInfo))
            {
                throw new ArgumentException("Invalid lambda expression");
            }
            return memberInfo;
        }
        private static PropertyInfo FindPropertyImplementation(PropertyInfo interfacePropertyInfo, Type actualType)
        {
            var interfaceType = interfacePropertyInfo.DeclaringType;

            var actualTypeInfo = actualType.GetTypeInfo();
            var bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
            var actualTypePropertyInfos = actualTypeInfo.GetMembers(bindingFlags).OfType<PropertyInfo>();

            var explicitlyImplementedPropertyName = $"{interfacePropertyInfo.DeclaringType.FullName}.{interfacePropertyInfo.Name}".Replace("+", ".");
            var explicitlyImplementedPropertyInfo = actualTypePropertyInfos
                .Where(p => p.Name == explicitlyImplementedPropertyName)
                .SingleOrDefault();
            if (explicitlyImplementedPropertyInfo != null)
            {
                return explicitlyImplementedPropertyInfo;
            }

            var implicitlyImplementedPropertyInfo = actualTypePropertyInfos
                .Where(p => p.Name == interfacePropertyInfo.Name && p.PropertyType == interfacePropertyInfo.PropertyType)
                .SingleOrDefault();
            if (implicitlyImplementedPropertyInfo != null)
            {
                return implicitlyImplementedPropertyInfo;
            }

            throw new ApplicationException($"Unable to find property info for property: '{interfacePropertyInfo.Name}'.");
        }
    }
}