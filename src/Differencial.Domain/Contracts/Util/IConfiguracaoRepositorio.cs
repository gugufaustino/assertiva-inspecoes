namespace Differencial.Domain.Contracts.Util
{
    public interface IConfiguracaoRepositorio
    {
        string NomeEmpresaCompleto { get; }
        string NomeEmpresaSimples { get; }
        string RepositorioOperadorImagem { get; }
        string PastaVirtualOperadorImagem { get; }
        string RepositorioAnexos { get; }
        string RepositorioSolicitacao { get; }
    }
}
