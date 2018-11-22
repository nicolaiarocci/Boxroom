using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;

namespace Boxroom.Database.Mongo
{
    public class MongoIgnoreDefaultPropertiesConvention : IMemberMapConvention
    {
        public string Name => $"Ignore Default Properties";

        public void Apply(BsonMemberMap memberMap)
        {
            memberMap.SetIgnoreIfDefault(true);
        }
    }
}