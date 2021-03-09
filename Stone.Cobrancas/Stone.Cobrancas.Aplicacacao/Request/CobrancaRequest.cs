using System;
using System.Text.Json.Serialization;

namespace Stone.Cobrancas.Aplicacacao.Request
{
    public class CobrancaRequest
    {
        [JsonPropertyName("data-vencimento")]
        public DateTime DataVencimento { get; set; }
        [JsonPropertyName("cpf")]
        public string Cpf { get; set; }
        [JsonPropertyName("valor-cobranca")]
        public decimal ValorCobranca { get; set; }
    }
}
