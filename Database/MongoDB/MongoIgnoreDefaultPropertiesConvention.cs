using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;

namespace Boxroom.Database
{
    /// <summary>
    /// Do not serialize type default values when saving to Mongo.
    /// Use this in with a ConventionPack, see the example below.
    /// <example>
    /// <code>
    /// var pack = new ConventionPack { new MongoIgnoreDefaultPropertiesConvention() };
    /// ConventionRegistry.Register("Ignore default properties", pack, _ => true);
    /// </code>
    /// </example>
    /// </summary>
    public class MongoIgnoreDefaultPropertiesConvention : IMemberMapConvention
    {
        public string Name => $"Ignore Default Properties";

        public void Apply(BsonMemberMap memberMap)
        {
            memberMap.SetIgnoreIfDefault(true);
        }
    }
}