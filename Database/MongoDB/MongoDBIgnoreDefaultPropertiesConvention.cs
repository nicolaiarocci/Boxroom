using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;

namespace DataStorage.Database.MongoDB
{
    public class MongoDBIgnoreDefaultPropertiesConvention : IMemberMapConvention
    {
        public string Name => $"Ignore Default Properties";

        public void Apply(BsonMemberMap memberMap)
        {
            memberMap.SetIgnoreIfDefault(true);
        }
    }
}