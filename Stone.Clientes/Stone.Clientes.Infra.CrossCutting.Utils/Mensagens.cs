using System.Collections.Generic;

namespace Stone.Clientes.Infra.CrossCutting.Utils
{
    public sealed class Mensagens
    {
        public Mensagens(in string mensagem, DetalhesDaMensagem detalhe)
        {
            Campos = new List<DetalhesDaMensagem>();
            Mensagem = mensagem;
            AdicionarMensagem(detalhe);
        }

        public Mensagens(in string mensagem, List<DetalhesDaMensagem> detalhes)
        {
            Mensagem = mensagem;
            Campos = detalhes ?? new List<DetalhesDaMensagem>();
        }
        public List<DetalhesDaMensagem> Campos { get; private set; }
        public string Mensagem { get; set; }
        public void AdicionarMensagem(DetalhesDaMensagem detail)
        {
            if (detail is object)
                Campos.Add(detail);
        }
    }
}
