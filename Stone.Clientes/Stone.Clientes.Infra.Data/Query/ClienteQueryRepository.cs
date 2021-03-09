using MongoDB.Driver;
using Stone.Clientes.Dominio.Entities;
using Stone.Clientes.Dominio.Repository.Interfaces;
using Stone.Clientes.Infra.Data.MongoDb.Configurations.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stone.Clientes.Infra.Data.Query
{
    public class ClienteQueryRepository : IClienteQueryRepository
    {
        private readonly IMongoDatabase _db;
        private readonly IClienteQueryRepositoryConfiguration _configuration;
        private const string COLLECTION_NAME = "cliente";
        public ClienteQueryRepository(IMongoDatabase mongoDatabase, IClienteQueryRepositoryConfiguration configuration)
        {
            _db = mongoDatabase;
            _configuration = configuration;
        }
        public async Task<Cliente> Consultar(string cpf)
        {
            var filtro = Builders<Cliente>.Filter.Where(x => x.Cpf == cpf);
            var cliente = await _db.GetCollection<Cliente>(COLLECTION_NAME)
                                .FindAsync(filtro);
            return cliente.FirstOrDefault();

        }

        public async Task<List<Cliente>> Consultar(int pagina)
        {
            int tamanhoPaginacao = _configuration.ObtenhaTamanhoConfiguracao();
            var filter = Builders<Cliente>.Filter.Empty;
            return await _db.GetCollection<Cliente>(COLLECTION_NAME).Find(filter)
                        .SortBy(x => x.Cpf)
                        .Skip((pagina - 1) * tamanhoPaginacao)
                        .Limit(tamanhoPaginacao)
                        .ToListAsync();
        }
    }
}
