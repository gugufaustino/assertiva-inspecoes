using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Domain.UOW;
using System.Collections.Generic;
using Differencial.Domain;
using Differencial.Domain.Contracts.Entities;
using Differencial.Domain.Util.ExtensionMethods;
using System;
using Differencial.Domain.Contracts.Infra;
using Differencial.Domain.Annotation;

namespace Differencial.Service.Services
{
    public class AtividadeProcessoService : Service, IAtividadeProcessoService
    {
        IAtividadeProcessoRepository _atividadeProcessoRepositorio;
        IUsuarioService _usuario;
        public AtividadeProcessoService(IUnitOfWork uow,
             IUsuarioService usuario,
            IAtividadeProcessoRepository atividadeProcessoRepositorio)
            : base(uow)
        {
            _atividadeProcessoRepositorio = atividadeProcessoRepositorio;
            _usuario = usuario;
        }

        public IEnumerable<AtividadeProcesso> Listar(AtividadeProcessoFilter filtro)
        {
            return TryCatch(() =>
            {
                return _atividadeProcessoRepositorio.Where(filtro);
            });
        }

        private void Salvar(AtividadeProcesso entidade)
        {
            TryCatch(() =>
            {
                entidade.Validate();

                if (entidade.Id == 0)
                    _atividadeProcessoRepositorio.Add(entidade);
                else
                    _atividadeProcessoRepositorio.Update(entidade);
            });
        }

        public List<AtividadeProcesso> Criar(IWorkFlowInstanciaProcesso processo)
        {
            List<AtividadeProcesso> lstAtividades = new List<AtividadeProcesso>();

            foreach (var tipoAtividade in Enum.GetValues(typeof(TipoAtividadeEnum)))
            {
                var atividadeAttribute = ((TipoAtividadeEnum)tipoAtividade).GetAttributeOfType<AtividadeAttribute>();

                if (!atividadeAttribute.IndAtividadeOpicional)
                {
                    var atividade = CriarNovaAtividade((TipoAtividadeEnum)tipoAtividade, processo.Id, atividadeAttribute.ShortName);
                    lstAtividades.Add(atividade);
                }
            }

            return lstAtividades;
        }

        public void Concluir(TipoAtividadeEnum tipoAtividade, IWorkFlowInstanciaProcesso processo)
        {

            var atividade = _atividadeProcessoRepositorio.FirstOrDefault(i => i.IdSolicitacao == processo.Id && i.TipoAtividade == tipoAtividade);
             
            atividade.IdOperadorConcluida = _usuario.Id;
            atividade.TipoSituacaoAtividade = TipoSituacaoAtividadeEnum.Concluida;
            atividade.DthConcluida = DateTime.Now;

            Salvar(atividade);


        }

        private AtividadeProcesso CriarNovaAtividade(TipoAtividadeEnum tipoAtividade, int IdProcesso, string nomeAtividade)
        {
           
                var atividade = new AtividadeProcesso
            {
                TipoSituacaoAtividade = TipoSituacaoAtividadeEnum.Nova,
                TipoAtividade = tipoAtividade,
                IdSolicitacao = IdProcesso,
                NomeAtividadeProcesso = nomeAtividade 
                };

            Salvar(atividade);
            return atividade;

        }

        public void Excluir(int[] ids)
        {
            TryCatch(() =>
            {
                _atividadeProcessoRepositorio.Delete(ids);
            });
        }

        public AtividadeProcesso Buscar(TipoAtividadeEnum tipoAtividade, IWorkFlowInstanciaProcesso processo)
        {
            return TryCatch(() =>
            {
            return _atividadeProcessoRepositorio.FirstOrDefault(i => i.TipoAtividade == tipoAtividade && i.IdSolicitacao == processo.Id);
            });
        }
    }
}