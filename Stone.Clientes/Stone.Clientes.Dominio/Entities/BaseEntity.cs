using System;

namespace Stone.Clientes.Dominio.Entities
{
    public abstract class BaseEntity
    {
        public BaseEntity()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; protected set; }
    }
}