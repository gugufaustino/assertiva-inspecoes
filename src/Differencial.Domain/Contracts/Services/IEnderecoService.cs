using Differencial.Domain.DTO;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using System.Collections.Generic;

namespace Differencial.Domain.Contracts.Services
{
    public interface IEnderecoService
    {
        IEnumerable<Endereco> Listar(EnderecoFilter filtro);

        void Salvar(Endereco entidade);

        void Excluir(int id);

        double? DistanciaRota(double latOrigem, double longOrigem, double latDestino, double longDestino);

        string URLMapaRota(double latOrigem, double longOrigem, double latDestino, double longDestino);

        GeoCoordinate BuscarGeoCordenadas(Endereco endereco);

        Endereco BuscarPorCEP(string cep);
    }
}