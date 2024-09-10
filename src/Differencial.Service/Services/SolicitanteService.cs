using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Entities;
using Differencial.Domain.Exceptions;
using Differencial.Domain.Filters;
using Differencial.Domain.Resources;
using Differencial.Domain.UOW;
using System.Collections.Generic;
using System.Linq;

namespace Differencial.Service.Services
{
    public class SolicitanteService : Service, ISolicitanteService
    {
        ISolicitanteRepository _solicitanteRepositorio;
         

        public SolicitanteService(IUnitOfWork uow,
                                ISolicitanteRepository solicitanteRepositorio)
            : base(uow)
        {
            _solicitanteRepositorio = solicitanteRepositorio; 
        }

        public IEnumerable<Solicitante> Listar(SolicitanteFilter filtro)
        {
            return TryCatch(() =>
            {
                return _solicitanteRepositorio.Where(filtro);
            });
        } 
        
        public IEnumerable<Solicitante> ddlSolicitante(int idSeguradora)
        {
            return TryCatch(() =>
            {
                return _solicitanteRepositorio.ddlSolicitante(idSeguradora);
            });
        }

        public void Salvar(Solicitante entidade)
        {
            TryCatch(() =>
            {
              
                if (entidade.Id == 0)
                    _solicitanteRepositorio.Add(entidade);
                else
                    _solicitanteRepositorio.Update(entidade);
            });
        }

        public void Excluir(int id)
        {
            TryCatch(() =>
            {
                var lstSolics = _solicitanteRepositorio.Find(id).Solicitacao.ToList();
                if (lstSolics.Any())
                    throw new ValidationException(MensagensValidacaoServicos.SolicitanteRnExisteSolicitacoes);
                 
                var operador = _solicitanteRepositorio.Find(id).Operador;
                operador.IndSolicitante = false;
                _solicitanteRepositorio.Delete(id);
            });
        }
    }
}