using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Domain.UOW;
using Differencial.Domain.Util.ExtensionMethods;
using System.Collections.Generic;
using System;

namespace Differencial.Service.Services
{
    public class ComunicacaoService : Service, IComunicacaoService
    {
        IComunicacaoRepository _comunicacaoRepositorio;
        IEmailService _emailService; 

        public ComunicacaoService(IUnitOfWork uow, IComunicacaoRepository comunicacaoRepositorio,
            IEmailService emailService)
            : base(uow)
        {
            _comunicacaoRepositorio = comunicacaoRepositorio;
            _emailService = emailService;
        }

        public IEnumerable<Comunicacao> Listar(ComunicacaoFilter filtro)
        {
            return TryCatch(() =>
            {
                return _comunicacaoRepositorio.Where(filtro);
            });
        }

        public void Salvar(Comunicacao entidade)
        {
            TryCatch(() =>
            {
                entidade.Validate();

                if (entidade.Id == 0)
                {
                    _comunicacaoRepositorio.Add(entidade); 
                }
                else
                    _comunicacaoRepositorio.Update(entidade); 
                
            });
        } 

        public void Excluir(int id)
        {
            TryCatch(() =>
            {
                _comunicacaoRepositorio.Delete(id);
            });
        }
    }
}