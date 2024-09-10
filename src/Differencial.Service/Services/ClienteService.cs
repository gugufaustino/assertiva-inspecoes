using Differencial.Domain.Contracts.Infra;
using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Domain.UOW;
using Differencial.Domain.Util.ExtensionMethods;
using System.Collections.Generic;
using System.Linq;

namespace Differencial.Service.Services
{
    public class ClienteService : Service, IClienteService
    {
        IClienteRepository _clienteRepositorio;
        IClienteEnderecoService _clienteEnderecoService;
        IUsuarioService _usuarioService;

        public ClienteService(IUnitOfWork uow, IClienteRepository clienteRepositorio,
           IClienteEnderecoService clienteEnderecoService,
            IUsuarioService usuarioService)
            : base(uow)
        {
            _clienteRepositorio = clienteRepositorio;
            _clienteEnderecoService = clienteEnderecoService;
            _usuarioService = usuarioService;

        }

        public IEnumerable<Cliente> Listar(ClienteFilter filtro)
        {
            return TryCatch(() =>
            {
                return _clienteRepositorio.Where(filtro);
            });
        }

        public void Salvar(Cliente entidade)
        {
            TryCatch(() =>
            {
                entidade.Validate();

                #region Tratamento Mínimo de Dados 
                entidade.ContatoOutro = entidade.ContatoOutro.TrimDefault();
                 
                #endregion Tratamento Mínimo de Dados

                if (entidade.Id == 0)
                    Inserir(_usuarioService.Id, entidade);
                else
                    Editar(_usuarioService.Id, entidade);

            });
        }

        public void Excluir( int id)
        {
            TryCatch(() =>
            {
                _clienteRepositorio.Delete(id);
            });
        }

        private void Inserir(int codigoUsuarioLogado, Cliente entidade)
        {

            if (entidade.ClienteEndereco.Any())
            {
                entidade.ClienteEndereco.ToList().ForEach(clienteEndereco =>
                {
                    _clienteEnderecoService.Salvar(codigoUsuarioLogado, clienteEndereco);
                });
            }

            _clienteRepositorio.Add(entidade);
        }
        private void Editar(int codigoUsuarioLogado, Cliente entidade)
        { 
            Cliente oldEntidade = _clienteRepositorio.Find(entidade.Id); 
            oldEntidade.CpfCnpj = entidade.CpfCnpj;
            oldEntidade.NomeRazaoSocial = entidade.NomeRazaoSocial;
            oldEntidade.AtividadeNome = entidade.AtividadeNome;
            oldEntidade.ContatoNome = entidade.ContatoNome;
            oldEntidade.ContatoTelefone = entidade.ContatoTelefone;
            oldEntidade.ContatoOutro = entidade.ContatoOutro;
            oldEntidade.ContatoAgendamento = entidade.ContatoAgendamento;
            _clienteRepositorio.Update(oldEntidade);
        }
    }
}