using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Boxroom.Core
{
    public class ClassMap
    {
        private readonly Type _classType;
        private readonly static Dictionary<Type, ClassMap> classMaps = new Dictionary<Type, ClassMap>();
        protected Dictionary<string, MemberMap> maps = new Dictionary<string, MemberMap>();
        public static List<string> ShouldAutoMapMembers { get; } = new List<string> {};

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
            if (classMaps.ContainsKey(classMap.ClassType))
            {
                return;
            }
            classMaps.Add(classMap.ClassType, classMap);
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

            var classMapDefinition = typeof(ClassMap<>);
            var classMapType = classMapDefinition.MakeGenericType(classType);
            classMap = (ClassMap) Activator.CreateInstance(classMapType);
            classMap.AutoMap();
            RegisterClassMap(classMap);

            return classMap;
        }

        protected void StoreMap(MemberMap memberMap, string memberName)
        {
            if (memberMap == null)
            {
                throw new ArgumentNullException(nameof(memberMap));
            }
            if (memberName == null)
            {
                throw new ArgumentNullException(nameof(memberName));
            }

            EnsureMemberMapIsForThisClass(memberMap);

            maps[memberName] = memberMap;
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
        public virtual void AutoMap()
        {
            foreach (var memberName in ShouldAutoMapMembers)
            {
                var memberInfo = _classType.GetTypeInfo().GetMember(memberName).FirstOrDefault();
                if (memberInfo == null) continue;

                var memberMap = MapMember(memberInfo);
                StoreMap(memberMap, memberName);
            }
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
        public MemberMap GetMap(string memberName)
        {
            if (memberName == null)
            {
                throw new ArgumentNullException(nameof(memberName));
            }

            MemberMap memberMap;
            if (maps.TryGetValue(memberName, out memberMap))
            {
                return memberMap;
            }
            return null;
        }
        public Type ClassType
        {
            get { return _classType; }
        }
    }
    public class ClassMap<TClass> : ClassMap
    {
        public ClassMap() : base(typeof(TClass)) {}

        public ClassMap(Action<ClassMap<TClass>> classMapInitializer) : base(typeof(TClass))
        {
            classMapInitializer(this);
        }
        public MemberMap MapMember<TMember>(Expression<Func<TClass, TMember>> memberLambda, string memberName)
        {
            var memberMap = MapMemberFromLambda(memberLambda);
            StoreMap(memberMap, memberName);
            return memberMap;
        }
        protected MemberMap MapMemberFromLambda<TMember>(Expression<Func<TClass, TMember>> lambda)
        {
            var memberInfo = GetMemberInfoFromLambda(lambda);
            return MapMember(memberInfo);
        }
        private static MemberInfo GetMemberInfoFromLambda<TMember>(Expression<Func<TClass, TMember>> memberLambda)
        {
            var body = memberLambda.Body;
            MemberExpression memberExpression;
            switch (body.NodeType)
            {
                case ExpressionType.MemberAccess:
                    memberExpression = (MemberExpression) body;
                    break;
                case ExpressionType.Convert:
                    var convertExpression = (UnaryExpression) body;
                    memberExpression = (MemberExpression) convertExpression.Operand;
                    break;
                default:
                    throw new ArgumentException("Invalid lambda expression");
            }
            var memberInfo = memberExpression.Member;
            if (memberInfo is PropertyInfo)
            {
                if (memberInfo.DeclaringType.GetTypeInfo().IsInterface)
                {
                    memberInfo = FindPropertyImplementation((PropertyInfo) memberInfo, typeof(TClass));
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