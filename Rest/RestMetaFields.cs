using DataStorage.Core;

namespace DataStorage.Rest
{
    public class RestMetaFields : MetaFields
    {
        public static string ETag { get; set; } = "ETag";
        public static string LastUpdated { get; set; } = "LastUpdated";
        public static string DateCreated { get; set; } = "DateCreated";
        public static string[] AsEnumerable()
        {
            return new string[] { ETag, LastUpdated, DateCreated };
        }
    }
}