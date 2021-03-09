using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Stone.ProcessamentoCobranca.Infra.CrossCutting.Utils
{
    public sealed class DetalhesDaMensagem
    {
        [JsonPropertyName("campo")]
        public string Campo { get; set; }
        [JsonPropertyName("mensagem")]
        public string Mensagem { get; set; }
        [JsonPropertyName("valor")]
        public string Valor { get; set; }
    }
}
