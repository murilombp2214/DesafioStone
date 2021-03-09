using Stone.ProcessamentoCobranca.Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Stone.ProcessamentoCobranca.Infra.Data.Clients.ApiCliente.Response
{
    public class ClienteStoneResponse
    {
        [JsonPropertyName("data")]
        public List<ClienteStoneResponseData> Data { get; set; }
    }
}
