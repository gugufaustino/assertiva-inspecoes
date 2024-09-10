using System;

namespace Differencial.Domain.Contracts.Entities
{
    public interface IEntity : IKeyId
    {
        //int Id { get; set; }
        DateTime DataCadastro { get; set; }
        DateTime DataModificacao { get; set; }
        int IdOperadorCadastro { get; set; }
        int IdOperadorModificacao { get; set; }

        void Validate();
    }
}