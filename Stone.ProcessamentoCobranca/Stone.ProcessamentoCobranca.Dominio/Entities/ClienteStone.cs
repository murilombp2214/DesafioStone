using System;
using System.Collections.Generic;
using System.Text;

namespace Stone.ProcessamentoCobranca.Dominio.Entities
{
    public class ClienteStone
    {
        public ClienteStone(Guid id, string nome, string estado, string cpf)
        { 
            Id = id;
            Nome = nome;
            Estado = estado;
            Cpf = cpf;
        }
        public Guid Id { get; private set; }
        public string Nome { get; private set; }
        public string Estado { get; private set; }
        public string Cpf { get; private set; }
    }
}
