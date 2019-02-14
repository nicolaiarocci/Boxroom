using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Boxroom.Rest
{
    internal class MemberVisitor : VisitorBase
    {
        private readonly MemberExpression node;
        private readonly VisitorOption position;
        public MemberVisitor(MemberExpression node, VisitorOption position) : base(node)
        {
            this.node = node;
            this.position = position;
        }

        public override string Visit()
        {
            if (this.position == VisitorOption.Left)
            {
                return node.Member.Name;
            }
            else
            {
                object value;
                Type type;
                switch (node.Member.MemberType)
                {
                    case MemberTypes.Field:
                        {
                            var fieldInfo = (FieldInfo)node.Member;
                            value = fieldInfo.GetValue((node.Expression as ConstantExpression).Value);
                            type = fieldInfo.FieldType;
                            break;
                        }
                    case MemberTypes.Property:
                        {
                            var propInfo = (PropertyInfo)node.Member;
                            var exp = (MemberExpression)node.Expression;
                            var constant = (ConstantExpression)exp.Expression;
                            var fieldInfo = ((FieldInfo)exp.Member).GetValue(constant.Value);
                            value = propInfo.GetValue(fieldInfo);
                            type = propInfo.PropertyType;
                            break;
                        }
                    default:
                        {
                            throw new NotSupportedException();
                        }
                }
                if (type == typeof(DateTime))
                {
                    // TODO this should be configurable.
                    return ((DateTime)value).ToString("r");
                }
                return value.ToString();
            }
        }
    }
}