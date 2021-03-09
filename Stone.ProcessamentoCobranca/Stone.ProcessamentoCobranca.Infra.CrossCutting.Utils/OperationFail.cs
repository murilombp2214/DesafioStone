using Stone.ProcessamentoCobranca.Infra.CrossCutting.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Stone.ProcessamentoCobranca.Infra.CrossCutting.Utils
{
    public sealed class OperationFail<T> : IOperation<T>
    {
        public OperationFail(in string mensagem, DetalhesDaMensagem detalhes)
        {
            Mensagens = new Mensagens(mensagem, detalhes);
        }

        public OperationFail(in string mensagem, List<DetalhesDaMensagem> detalhes)
        {
            Mensagens = new Mensagens(mensagem, detalhes);
        }

        [JsonPropertyName("mensagens")]
        public Mensagens Mensagens { get; private set; }
    }
}
