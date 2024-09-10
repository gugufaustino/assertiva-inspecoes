using Differencial.Domain.Contracts.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using static Differencial.Domain.GoogleMapsResponse;

namespace Differencial.Service.ServiceUtility
{

    public class GoogleMapsService : IGoogleMapsService
    {
        private readonly string key;
        private readonly HttpClient httpClient;

        public GoogleMapsService(HttpClient httpClient, IConfiguracaoAplicativo configuracaoAplicativo)
        {
            httpClient.BaseAddress =  new Uri($"https://maps.googleapis.com/maps/api/");
            this.key = configuracaoAplicativo.GoogleApiKey;
            this.httpClient = httpClient;
        }

        public async Task<ServiceResponse<Location>> GetLocation(string address)
        {
            var response = await httpClient.GetAsync($"geocode/json?key={key}&address={address}");
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Root>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return new ServiceResponse<Location>()
            {
                Status = result.status,
                Result = result.status == "OK" ? result.results.First().Geometry.location : null,
                Error = result.error_message                
            };
        }
        public async Task<ServiceResponse<List<AddressComponent>>> GetAddress(string address)
        {
            var response = await httpClient.GetAsync($"geocode/json?key={key}&address={address}");
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Root>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return new ServiceResponse<List<AddressComponent>>()
            {
                Status = result.status,
                Result = result.status == "OK" ? result.results.First().Address_components : null,
                Error = result.error_message                
            };
        }

        public async Task<ServiceResponse<Leg>> GetRoute(Location destination, Location origin)
        {
            //origin=41.43206,-81.38992
            var sDestination = $"{destination.lat.ToStringInvariant()},{destination.lng.ToStringInvariant()}";
            var sOrigin = $"{origin.lat.ToStringInvariant()},{origin.lng.ToStringInvariant()}";
            
            var response = await httpClient.GetAsync($"directions/json?key={key}&destination={sDestination}&origin={sOrigin}");
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Root>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return new ServiceResponse<Leg>()
            {
                Status = result.status,
                Result = result.status == "OK" ? result.routes.First().Legs.First() : null,
                Error = result.error_message
            };
        }
    }

    internal static class GoogleMapsExtensions
    {
      
        public static string ToStringInvariant(this double value)
        {
            return value.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
