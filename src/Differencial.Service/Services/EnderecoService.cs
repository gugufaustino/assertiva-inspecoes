using Differencial.Domain;
using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.DTO;
using Differencial.Domain.Entities;
using Differencial.Domain.Exceptions;
using Differencial.Domain.Filters;
using Differencial.Domain.Resources;
using Differencial.Domain.UOW;
using Differencial.Domain.Util.ExtensionMethods;
using Differencial.Service.ServiceUtility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Differencial.Service.Services
{
    public class EnderecoService : Service, IEnderecoService
    {
        private readonly IEnderecoRepository _enderecoRepositorio;
        private readonly IGoogleMapsService googleMapsService;

        public EnderecoService(IUnitOfWork uow,
            IEnderecoRepository enderecoRepositorio,
            IGoogleMapsService googleMapsService)
            : base(uow)
        {
            _enderecoRepositorio = enderecoRepositorio;
            this.googleMapsService = googleMapsService;
        }

        public IEnumerable<Endereco> Listar(EnderecoFilter filtro)
        {
            return TryCatch(() =>
            {
                return _enderecoRepositorio.Where(filtro);
            });
        }

        public async void Salvar(Endereco entidade)
        {


            entidade.Validate();
            #region Tratamento Mínimo de Dados

            entidade.Cep = entidade.Cep.IsNullOrEmpty() ? entidade.Cep : entidade.Cep.RemoveNonNumbers();

            entidade.NomeMunicipio = entidade.NomeMunicipio.IsNullOrEmpty() ? entidade.NomeMunicipio : entidade.NomeMunicipio.Trim();
            entidade.SiglaUf = entidade.SiglaUf.IsNullOrEmpty() ? entidade.SiglaUf : entidade.SiglaUf.Trim().ToUpper();
            entidade.Bairro = entidade.Bairro.IsNullOrEmpty() ? entidade.Bairro : entidade.Bairro.Trim();
            entidade.Logradouro = entidade.Logradouro.IsNullOrEmpty() ? entidade.Logradouro : entidade.Logradouro.Trim();
            #endregion Tratamento Mínimo de Dados



            if (entidade.Id == 0)
                _enderecoRepositorio.Add(entidade);
            else
            {
                Endereco oldEntidade = await _enderecoRepositorio.FindAsync(entidade.Id);
                oldEntidade.Logradouro = entidade.Logradouro;
                oldEntidade.Bairro = entidade.Bairro;
                oldEntidade.Cep = entidade.Cep;
                oldEntidade.Complemento = entidade.Complemento;
                oldEntidade.Latitude = entidade.Latitude;
                oldEntidade.Longitude = entidade.Longitude;
                oldEntidade.NomeMunicipio = entidade.NomeMunicipio;
                oldEntidade.Numero = entidade.Numero;
                oldEntidade.SiglaUf = entidade.SiglaUf;
                _enderecoRepositorio.Update(oldEntidade);
            }


        }

        public void Excluir(int id)
        {
            TryCatch(() =>
            {
                _enderecoRepositorio.Delete(id);
            });
        }

        public double? DistanciaRota(double latOrigem, double longOrigem, double latDestino, double longDestino)
        {

            var response = googleMapsService.GetRoute(new GoogleMapsResponse.Location(latOrigem, longOrigem), new GoogleMapsResponse.Location(latDestino, longDestino)).Result;
            if (response.Status == "OK")
            {
                return response.Result.Distance.Value / 1000d;
            }
            else if (response.Status == "ZERO_RESULTS" || response.Status == "INVALID_REQUEST" || response.Status == "NOT_FOUND")
            {
                throw new ValidationException(MensagensValidacaoServicos.RnDistanciaRotaZeroResults);
            }
            else
            {
                throw new ServiceException(MensagensValidacaoServicos.ServExGoogleApiGeoResul.Formata(response.Status));
            }
        }

        public string URLMapaRota(double latOrigem, double longOrigem, double latDestino, double longDestino)
        {
            string strUrlMapa = "http://maps.google.com.br/maps?saddr={0}&daddr={1}&ie=UTF8&t=m&z=5&layer=m&mode=driving&units=metric"
                                    .Formata(string.Format(new CultureInfo("en-US"), "{0}, {1}", latOrigem, longOrigem),
                                                string.Format(new CultureInfo("en-US"), "{0}, {1}", latDestino, longDestino));
            return strUrlMapa;
        }


        public GeoCoordinate BuscarGeoCordenadas(Endereco endereco)
        {

            string strEndereco;
            if (endereco.Numero.HasValue)
                strEndereco = string.Format("{0}, {1} - {2}, {3} - {4} - Brazil", endereco.Logradouro, endereco.Numero, endereco.Bairro, endereco.NomeMunicipio, endereco.SiglaUf);
            else
            {
                if (endereco.Logradouro.Contains("2 TORRES"))
                    endereco.Logradouro = endereco.Logradouro.Replace("2 TORRES", "");


                strEndereco = string.Format("{0}, {1} - {2} - Brazil", endereco.Logradouro, endereco.NomeMunicipio, endereco.SiglaUf);
            }

            var response = googleMapsService.GetLocation(strEndereco).Result;

            if (response.Status == "OK")
            {
                var addressLocation = response.Result;

                return new GeoCoordinate(addressLocation.lat, addressLocation.lng);
            }
            else if (response.Status == "ZERO_RESULTS" || response.Status == "INVALID_REQUEST")
            {
                throw new ValidationException(MensagensValidacaoServicos.RnEnderecoBuscarGeoZeroResult);
            }
            else
            {
                throw new ServiceException(MensagensValidacaoServicos.ServExGoogleApiGeoResul.Formata(response.Status));
            }
        }

        public Endereco BuscarPorCEP(string cep)
        {
            var response = googleMapsService.GetAddress(cep).Result;

            if (response.Status != "ZERO_RESULTS")
            {


                var addressInformation = response.Result;

                var results = addressInformation.FirstOrDefault(x => x.types.Contains("street_address") || x.types.Contains("postal_code"));

                if (results != null)
                {
                    var endereco = new Endereco();
                    var resultBairro = addressInformation.FirstOrDefault(a => a.types.Contains("sublocality_level_1"));
                    if (resultBairro != null)
                        endereco.Bairro = resultBairro.long_name;

                    var resultLogradouro = addressInformation.FirstOrDefault(a => a.types.Contains("route"));
                    if (resultLogradouro != null)
                        endereco.Logradouro = resultLogradouro.long_name;

                    endereco.SiglaUf = addressInformation.FirstOrDefault(a => a.types.Contains("administrative_area_level_1")).short_name;
                    endereco.NomeMunicipio = addressInformation.FirstOrDefault(a => a.types.Contains("locality") || a.types.Contains("administrative_area_level_2")).long_name;

                    return endereco;
                }
            }

            return null;
        }
         
    }
}