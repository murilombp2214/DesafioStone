using MongoDB.Bson;
using MongoDB.Driver;

namespace Stone.Clientes.Infra.Data.MongoDb
{
    public class MongoDatabaseProvider
    {
        public static IMongoDatabase GetDatabase(string connectionString, string databaseName)
        {
            var mongoCliente = new MongoClient(connectionString);
            return mongoCliente.GetDatabase(databaseName);
        }
    }
}
