using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Domain.UOW;
using System.Linq;
using System.Collections.Generic;
using Differencial.Domain.Exceptions;
using System;
using Differencial.Domain.DTO;
using Differencial.Domain.Queries;

namespace Differencial.Service.Services
{
    public class VistoriadorService : Service, IVistoriadorService
    {
        private IVistoriadorRepository _vistoriadorRepositorio;
        private IVistoriadorProdutoService _vistoriadorProdutoService;
        private IVistoriadorQueries _dpvistoriadorRepository;

        public VistoriadorService(IUnitOfWork uow, IVistoriadorRepository vistoriadorRepositorio, IVistoriadorQueries dpvistoriadorRepository,
            IVistoriadorProdutoService vistoriadorProdutoService)
            : base(uow)
        {
            _vistoriadorRepositorio = vistoriadorRepositorio;
            _vistoriadorProdutoService = vistoriadorProdutoService;
            _dpvistoriadorRepository = dpvistoriadorRepository;


        }
        public IEnumerable<Vistoriador> Listar(VistoriadorFilter filtro)
        {
            return TryCatch(() =>
            {
                return _vistoriadorRepositorio.Where(filtro);
            });
        }
        public IEnumerable<Vistoriador> ListarVistoriadorPorProduto(int idProduto)
        {
            return TryCatch(() =>
            {
                return _vistoriadorRepositorio.ListarVistoriadorPorProduto(idProduto);                            
            });
        }
        public void Salvar(int codigoUsuarioLogado, Vistoriador entidade)
        {
            TryCatch(() =>
            {
                entidade.Validate();

                if (entidade.Id == 0)
                    _vistoriadorRepositorio.Add(entidade);
                else
                    _vistoriadorRepositorio.Update(entidade);
            });
        }
        public void Excluir(int codigoUsuarioLogado, int id)
        {
            TryCatch(() =>
            {

                if (_vistoriadorRepositorio.Find(id).VistoriadorProduto.Any())
                    throw new ValidationException("Vistoriador possui produtos já registrados e então não pode mais ser excluído");

                _vistoriadorRepositorio.Delete(id);
            });
        }

        public Vistoriador Buscar(int id)
        {
            return TryCatch(() =>
            {
                return _vistoriadorRepositorio.Buscar(id);
            });
        }

        public List<OperadorDistancia> ListarOperadorDistancia(double latitude, double longitude, int idProduto, int? IdContratoLancamentoValor)
        {
            return TryCatch(() =>
             {
                 var lstOperadores = _dpvistoriadorRepository.ListarOperadorDistancia(latitude, longitude, idProduto, IdContratoLancamentoValor ?? 0);

                 var lstIdOperadores = lstOperadores.Select(i => i.Id).ToList();

                 var lstVistoriadores = _vistoriadorRepositorio.ListarVistoriadorOperador(lstIdOperadores);

                 foreach (var operadorDistanciaRetorno in lstOperadores)
                 {
                     operadorDistanciaRetorno.Operador = lstVistoriadores.First(v => v.Id == operadorDistanciaRetorno.Id).Operador;
                 }

                 return lstOperadores;
             });

        }
    }
}