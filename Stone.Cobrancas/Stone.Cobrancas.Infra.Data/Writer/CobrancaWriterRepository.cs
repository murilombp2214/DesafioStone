using MongoDB.Driver;
using Stone.Cobrancas.Dominio.Entities;
using Stone.Cobrancas.Dominio.Repository.Interface;
using System.Threading.Tasks;

namespace Stone.Cobrancas.Infra.Data.Writer
{
    public class CobrancaWriterRepository : ICobrancaWriterRepository
    {
        private readonly IMongoDatabase _db;
        private const string COLLECTION_NAME = "cobranca";
        public CobrancaWriterRepository(IMongoDatabase mongoDatabase)
        {
            _db = mongoDatabase;
        }
        public async Task<Cobranca> Cadastrar(Cobranca cobranca)
        {
            await _db.GetCollection<Cobranca>(COLLECTION_NAME).InsertOneAsync(cobranca);
            return cobranca;
        }
    }
}
