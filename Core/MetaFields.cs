using System.Collections.Generic;
using System.Reflection;

namespace Boxroom.Core
{
    public class MetaFields
    {
        public string Id { get; set; } = "Id";
        public string LastUpdated { get; set; } = "LastUpdated";
        public List<string> AsList()
        {
            var metaFields = new List<string>();
            foreach (var memberInfo in GetType().GetProperties())
            {
                metaFields.Add(memberInfo.GetValue(this).ToString());
            }
            return metaFields;
        }
    }
}