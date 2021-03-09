using Stone.ProcessamentoCobranca.Infra.CrossCutting.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stone.ProcessamentoCobranca.Infra.CrossCutting.Utils
{
    public static class Result
    {
        public static IOperation<T> CreateSuccess<T>()
        {
            return new OperationSuccess<T>(default);
        }

        public static IOperation<T> CreateSuccess<T>(T value)
        {
            return new OperationSuccess<T>(value);
        }

        public static IOperation<T> CreateFailure<T>(in string mensagem, DetalhesDaMensagem detalhe)
        {
            return new OperationFail<T>(mensagem, detalhe);
        }

        public static IOperation<T> CreateFailure<T>(in string mensagem)
        {
            return new OperationFail<T>(mensagem, default(DetalhesDaMensagem));
        }

        public static IOperation<T> CreateFailure<T>(in string mensagem, List<DetalhesDaMensagem> detalhes)
        {
            return new OperationFail<T>(mensagem, detalhes);
        }

    }
}
