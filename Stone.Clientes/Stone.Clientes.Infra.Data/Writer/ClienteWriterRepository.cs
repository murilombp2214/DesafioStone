using MongoDB.Driver;
using Stone.Clientes.Dominio.Entities;
using Stone.Clientes.Dominio.Repository.Interfaces;
using System.Threading.Tasks;

namespace Stone.Clientes.Infra.Data.Repository
{
    public class ClienteWriterRepository : IClienteWriterRepository
    {
        private readonly IMongoDatabase _db;
        private const string COLLECTION_NAME = "cliente";
        public ClienteWriterRepository(IMongoDatabase mongoDatabase)
        {
            _db = mongoDatabase;
        }

        public async Task<Cliente> Cadastrar(Cliente cartao)
        {
            await _db.GetCollection<Cliente>(COLLECTION_NAME).InsertOneAsync(cartao);
            return cartao;
        }
    }
}
