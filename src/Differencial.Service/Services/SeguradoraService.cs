using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Domain.UOW;
using System.Collections.Generic;

namespace Differencial.Service.Services
{
    public class SeguradoraService : Service, ISeguradoraService
    {
        ISeguradoraRepository _seguradoraRepositorio;
        IEnderecoService _enderecoService;

        public SeguradoraService(IUnitOfWork uow, ISeguradoraRepository seguradoraRepositorio, IEnderecoService enderecoService)
            : base(uow)
        {
            _seguradoraRepositorio = seguradoraRepositorio;
            _enderecoService = enderecoService;
        }

        public IEnumerable<Seguradora> Listar(SeguradoraFilter filtro)
        {
            return TryCatch(() =>
            {
                return _seguradoraRepositorio.Where(filtro);
            });
        }

        public void Salvar(Seguradora entidade)
        {
            TryCatch(() =>
            {
                entidade.Validate();

                if (entidade.Id == 0)
                {
                    entidade.IndAtivo = true;
                    _enderecoService.Salvar(entidade.Endereco);

                    _seguradoraRepositorio.Add(entidade);
                }
                else
                {
                    Seguradora oldEntidade = this.Buscar((int)entidade.Id);

                    oldEntidade.NomeSeguradora = entidade.NomeSeguradora;
                    oldEntidade.Cnpj = entidade.Cnpj;
                    oldEntidade.RazaoSocial = entidade.RazaoSocial;
                    oldEntidade.Inscricao = entidade.Inscricao;
                    oldEntidade.ContabilInspecoesDiaInicio = entidade.ContabilInspecoesDiaInicio;
                    oldEntidade.ContabilInspecoesDiaFim = entidade.ContabilInspecoesDiaFim;
                    oldEntidade.ContabilInspetorDia = entidade.ContabilInspetorDia;
                    oldEntidade.ContabilEmpresaDia = entidade.ContabilEmpresaDia;

                    oldEntidade.IndAgendaRepostaPorEmail = entidade.IndAgendaRepostaPorEmail;
                    oldEntidade.IndLaudoRepostaPorEmail = entidade.IndLaudoRepostaPorEmail;
                    oldEntidade.IndIntegracaoSolicitacaoPorEmail = entidade.IndIntegracaoSolicitacaoPorEmail;
                    oldEntidade.EmailRemetenteSolicitacao = entidade.EmailRemetenteSolicitacao;

                    oldEntidade.QtdQuilometroFranquia = entidade.QtdQuilometroFranquia;
                    oldEntidade.VlrQuilometroExcedente = entidade.VlrQuilometroExcedente;

                    _enderecoService.Salvar(entidade.Endereco);

                    _seguradoraRepositorio.Update(oldEntidade);
                }
            });
        }

        public void Excluir(int id)
        {
            TryCatch(() =>
            {
                _seguradoraRepositorio.Delete(id);
            });
        }

        public Seguradora Buscar(int id)
        {
            return TryCatch(() =>
            {
                return _seguradoraRepositorio.Find(id);
            });
        }
        public void Excluir(int[] ids)
        {
            TryCatch(() =>
            {
                _seguradoraRepositorio.Delete(ids);
            });
        }

        public Seguradora ObterPorRemetenteSolicitacao(string emailremetentesolicitacao)
        {
            return TryCatch(() =>
            {
                return _seguradoraRepositorio.FirstOrDefault(s => s.EmailRemetenteSolicitacao == emailremetentesolicitacao);
            });
        }

    }
}