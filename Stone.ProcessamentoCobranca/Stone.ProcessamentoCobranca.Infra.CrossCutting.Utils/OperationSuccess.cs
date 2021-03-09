using Stone.ProcessamentoCobranca.Infra.CrossCutting.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Stone.ProcessamentoCobranca.Infra.CrossCutting.Utils
{
    public sealed class OperationSuccess<T> : IOperation<T>
    {
        public OperationSuccess(T data)
        {
            Data = data;
        }
        [JsonPropertyName("data")]
        public T Data { get; }
    }
}
