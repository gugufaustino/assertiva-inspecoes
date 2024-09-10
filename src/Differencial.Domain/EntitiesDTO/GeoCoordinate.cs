namespace Differencial.Domain.DTO
{
    public class GeoCoordinate
    {
        public double Latitude;
        public double Longitude;

        public GeoCoordinate()
        {

        }

        public GeoCoordinate(double latitude, double longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }
    }
}