using System.Collections.Generic;
using System.Threading.Tasks;
using Differencial.Domain;

namespace Differencial.Service.ServiceUtility
{
    public interface IGoogleMapsService
    {
        Task<GoogleMapsResponse.ServiceResponse<GoogleMapsResponse.Location>> GetLocation(string address);
        Task<GoogleMapsResponse.ServiceResponse<GoogleMapsResponse.Leg>> GetRoute(GoogleMapsResponse.Location origin, GoogleMapsResponse.Location destination);
        Task<GoogleMapsResponse.ServiceResponse<List<GoogleMapsResponse.AddressComponent>>> GetAddress(string address);
    }
 
}
