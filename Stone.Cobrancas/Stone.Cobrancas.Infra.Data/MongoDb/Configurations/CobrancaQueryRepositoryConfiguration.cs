using Stone.Cobrancas.Infra.Data.MongoDb.Configurations.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stone.Cobrancas.Infra.Data.MongoDb.Configurations
{

    public class CobrancaQueryRepositoryConfiguration : ICobrancaQueryRepositoryConfiguration
    {
        private readonly int _tamanhoPaginacao;

        public CobrancaQueryRepositoryConfiguration(int tamahoPaginacao)
        {
            _tamanhoPaginacao = tamahoPaginacao == 0 ? 10 : tamahoPaginacao;
        }
        public int ObtenhaTamanhoConfiguracao()
        {
            return _tamanhoPaginacao;
        }
    }

}
