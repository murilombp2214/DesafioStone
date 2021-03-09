using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Stone.ProcessamentoCobranca.Infra.Data.Clients.ApiCliente.Response
{
    public class ClienteStoneResponseData
    {
        [JsonPropertyName("id")]
        public Guid Id { get;  set; }
        [JsonPropertyName("nome")]
        public string Nome { get;  set; }
        [JsonPropertyName("estado")]
        public string Estado { get;  set; }
        [JsonPropertyName("cpf")]
        public string Cpf { get;  set; }
    }
}
