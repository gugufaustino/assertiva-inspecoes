namespace Differencial.Domain.Contracts.Entities
{
    public interface IAtivavel
    {
        bool IndAtivo { get; set; }
    }

    public interface IAtivavelFilter
    {
        bool? IndAtivo { get; set; }
    }
}
