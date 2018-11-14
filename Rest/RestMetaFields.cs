using DataStorage.Core;

namespace DataStorage.Rest
{
    public class RestMetaFields : MetaFields
    {
        public static string ETag { get; set; } = "ETag";
        public static string DateCreated { get; set; } = "DateCreated";
    }
}