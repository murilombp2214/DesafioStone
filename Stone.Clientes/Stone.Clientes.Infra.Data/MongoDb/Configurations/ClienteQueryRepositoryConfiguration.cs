using Stone.Clientes.Infra.Data.MongoDb.Configurations.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stone.Clientes.Infra.Data.MongoDb.Configurations
{
    public class ClienteQueryRepositoryConfiguration : IClienteQueryRepositoryConfiguration
    {
        private readonly int _tamanhoPaginacao;

        public ClienteQueryRepositoryConfiguration(int tamahoPaginacao)
        {
            _tamanhoPaginacao = tamahoPaginacao == 0 ? 10 : tamahoPaginacao;
        }
        public int ObtenhaTamanhoConfiguracao()
        {
            return _tamanhoPaginacao;
        }
    }
}
