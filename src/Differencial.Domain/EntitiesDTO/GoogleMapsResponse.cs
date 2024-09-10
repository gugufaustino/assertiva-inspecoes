using System.Collections.Generic;

namespace Differencial.Domain
{
    public class GoogleMapsResponse
    {
        public class ServiceResponse<T>
         where T : new()
        {
            public string Status { get; set; }
            public string Error { get; set; }
            public T Result { get; set; }

        }
        public class Location
        {
            public Location()
            {
            }

            public Location(double lat, double lng)
            {
                this.lat = lat;
                this.lng = lng;
            }
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class Root
        {
            public List<Result> results { get; set; }
            public List<Route> routes { get; set; }
            public string status { get; set; }
            public string error_message { get; set; }

        }

        public class Result
        {
            public List<AddressComponent> Address_components { get; set; }
            public string Formatted_address { get; set; }
            public Geometry Geometry { get; set; }

        }


        public class AddressComponent
        {
            public string long_name { get; set; }
            public string short_name { get; set; }
            public List<string> types { get; set; }
        }

        public class Geometry
        {
            public Location location { get; set; }
            public string location_type { get; set; }
            public Viewport viewport { get; set; }
        }

        public class Route
        {
            public Bounds Bounds { get; set; }
            public List<Leg> Legs { get; set; }
            public List<object> warnings { get; set; }
            public List<object> waypoint_order { get; set; }
        }
        public class Leg
        {
            public Distance Distance { get; set; }
            public Duration Duration { get; set; }
            public string End_address { get; set; }
            public Location End_location { get; set; }
            public string Start_address { get; set; }
            public Location Start_location { get; set; }

        }

        public class Bounds
        {
            public Northeast northeast { get; set; }
            public Southwest southwest { get; set; }
        }

        public class Distance
        {
            public string Text { get; set; }
            public int Value { get; set; }
        }

        public class Duration
        {
            public string Text { get; set; }
            public int Value { get; set; }
        }


        public class Northeast
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class Southwest
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class Viewport
        {
            public Northeast northeast { get; set; }
            public Southwest southwest { get; set; }
        }
    }
}