namespace Differencial.Domain.Contracts.Util
{
    public interface IConfiguracaoAplicativo : IConfiguracaoRepositorio
    { 
        string DominioAplicativo { get; } 
        string ConnectionString { get; }
        string AppVersao { get; }
        public string UsuarioRoot { get; }
        public string UsuarioRootPwd { get; }
        string GoogleApiKey { get;   }
    }
}
