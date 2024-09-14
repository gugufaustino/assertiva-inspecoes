namespace Differencial.Domain.Contracts.Infra
{
    public interface ICache 
    {
        void Definir(string chave, object obj);

        object Buscar(string chave);
    }
}
