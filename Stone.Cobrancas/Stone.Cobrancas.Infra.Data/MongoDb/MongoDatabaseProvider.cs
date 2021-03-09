using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stone.Cobrancas.Infra.Data.MongoDb
{
    public static class MongoDatabaseProvider
    {
        public static IMongoDatabase GetDatabase(string connectionString, string databaseName)
        {
            var mongoCliente = new MongoClient(connectionString);
            return mongoCliente.GetDatabase(databaseName);
        }
    }
}
