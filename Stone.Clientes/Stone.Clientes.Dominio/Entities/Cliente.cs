using System;

namespace Stone.Clientes.Dominio.Entities
{
    public class Cliente : BaseEntity
    {
        public Cliente(string nome, string estado,string cpf)
        {
            Nome = nome;
            Estado = estado;
            Cpf = cpf;
        }

        public Cliente(Guid id, string nome, string estado, string cpf) : this(nome,estado,cpf)
        {
            Id = id;
        }

        public string Nome { get; private set; }
        public string Estado { get; private set; }
        public string Cpf { get; private set; }

    }
}
