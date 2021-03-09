using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Stone.ProcessamentoCobranca.Infra.Data.Clients.ApiCobranca.Request
{
    public class CobrancaStoneRequest
    {
        [JsonPropertyName("data-vencimento")]
        public DateTime DataVencimento { get; set; }
        [JsonPropertyName("cpf")]
        public string Cpf { get; set; }
        [JsonPropertyName("valor-cobranca")]
        public decimal ValorCobranca { get; set; }
    }
}
