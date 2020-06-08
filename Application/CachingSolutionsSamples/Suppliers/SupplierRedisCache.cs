using NorthwindLibrary;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CachingSolutionsSamples.Suppliers
{
    public class SupplierRedisCache : IEntityCache<Supplier>
	{
		private ConnectionMultiplexer redisConnection;
		string prefix = "Cache_Suppliers";
		DataContractSerializer serializer = new DataContractSerializer(
			typeof(IEnumerable<Supplier>));

		public SupplierRedisCache(string hostName)
		{
			redisConnection = ConnectionMultiplexer.Connect(hostName);
		}

		public void Delete(string forUser)
		{
			var db = redisConnection.GetDatabase();
			db.KeyDelete(prefix + forUser);
		}

		public IEnumerable<Supplier> Get(string forUser)
		{
			var db = redisConnection.GetDatabase();
			byte[] s = db.StringGet(prefix + forUser);
			if (s == null)
				return null;

			return (IEnumerable<Supplier>)serializer
				.ReadObject(new MemoryStream(s));

		}

		public void Set(string forUser, IEnumerable<Supplier> categories)
		{
			var db = redisConnection.GetDatabase();
			var key = prefix + forUser;

			if (categories == null)
			{
				db.StringSet(key, RedisValue.Null, TimeSpan.FromMilliseconds(300));
			}
			else
			{
				var stream = new MemoryStream();
				serializer.WriteObject(stream, categories);
				db.StringSet(key, stream.ToArray(), TimeSpan.FromMilliseconds(300));
			}
		}
	}
}
