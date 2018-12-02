using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Boxroom.Database
{
    public partial class RedisBox : DatabaseBox, IRedisBox
    {
        private IConnectionMultiplexer redis;
        public RedisBox()
        {
            ConnectionString = "localhost";
        }
        public RedisBox(IConnectionMultiplexer multiplexer) : this()
        {
            redis = multiplexer;
        }
        public override void ValidateProperties()
        {

            if (ConnectionString == null)
            {
                throw new ArgumentNullException(nameof(ConnectionString));
            }
        }
        private int GetDatabaseIndex()
        {
            if (DataBaseName == null) return 0;
            if (!int.TryParse(DataBaseName, out int index))
            {
                // TODO: localized explanation: redis needs numeric db indexes
                throw new ArgumentException(nameof(DataBaseName));
            }
            return index;
        }
        private void EnsureConnection()
        {
            if (redis == null)
            {
                redis = ConnectionMultiplexer.Connect(ConnectionString);
            }
        }
    }
}
