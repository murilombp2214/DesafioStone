using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Stone.Cobrancas.Dominio.Entities;
using Stone.Cobrancas.Dominio.Repository.Interface;
using Stone.Cobrancas.Infra.Data.MongoDb.Configurations.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Stone.Cobrancas.Infra.Data.Query
{
    public class CobrancaQueryRepository : ICobrancaQueryRepository
    {
        private const string COLLECTION_NAME = "cobranca";
        private const string NOME_CAMPO_AGREGRACAO = "Month";
        private static readonly BsonDocument camposAgregacaoConsultaPorMes = BsonDocument.Parse("{$addFields: {Month : { $month : '$DataVencimento'}  }}");

        private readonly IMongoDatabase _db;
        private readonly ICobrancaQueryRepositoryConfiguration _configuration;
        
        public CobrancaQueryRepository(IMongoDatabase mongoDatabase, ICobrancaQueryRepositoryConfiguration configuration)
        {
            _db = mongoDatabase;
            _configuration = configuration;
        }

        public async Task<List<Cobranca>> ConsultarCobrancas(string cpf, int pagina)
        {
            int tamanhoPaginacao = _configuration.ObtenhaTamanhoConfiguracao();
            var filter = Builders<Cobranca>.Filter.Where(x => x.Cpf == cpf);
            return await _db.GetCollection<Cobranca>(COLLECTION_NAME).Find(filter)
                        .SortBy(x => x.Cpf)
                        .Skip((pagina - 1) * tamanhoPaginacao)
                        .Limit(tamanhoPaginacao)
                        .ToListAsync();
        }

        public Task<List<Cobranca>> ConsultarCobrancas(int mes, int pagina)
        {
            int tamanhoPaginacao = _configuration.ObtenhaTamanhoConfiguracao();
            var aggregate = _db.GetCollection<Cobranca>(COLLECTION_NAME)
                            .Aggregate()
                            .SortBy(x => x.DataVencimento)
                            .AppendStage<BsonDocument>(camposAgregacaoConsultaPorMes)
                            .Match(new BsonDocument(NOME_CAMPO_AGREGRACAO, mes))
                            .Project(Builders<BsonDocument>.Projection.Exclude(NOME_CAMPO_AGREGRACAO))
                            .Skip((pagina - 1) * tamanhoPaginacao)
                            .Limit(tamanhoPaginacao);

            return Task.FromResult(aggregate.ToList().Select(x => BsonSerializer.Deserialize<Cobranca>(x)).ToList());               
        }
    }
}
