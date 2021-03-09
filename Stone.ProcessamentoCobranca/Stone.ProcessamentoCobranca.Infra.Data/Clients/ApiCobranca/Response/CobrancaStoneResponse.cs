using System;
using System.Collections.Generic;
using System.Text;

namespace Stone.ProcessamentoCobranca.Infra.Data.Clients.ApiCobranca.Response
{
    public class CobrancaStoneResponse
    {
        public Guid Id { get; set; }
        public DateTime DataVencimento { get; set; }
        public string Cpf { get; set; }
        public decimal ValorCobranca { get; set; }
    }
}
