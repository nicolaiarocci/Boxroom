using Boxroom.Core;

namespace Boxroom.Rest
{
    public class MetaFields : Core.MetaFields
    {
        public string ETag { get; set; } = "ETag";
        public string DateCreated { get; set; } = "DateCreated";
    }
}