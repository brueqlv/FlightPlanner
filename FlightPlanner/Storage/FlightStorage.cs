using FlightPlanner.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FlightPlanner.Storage
{
    public class FlightStorage
    {
        private static List<Flight> _flights = new();
        private static int _id;

        public static void AddFlight(Flight flight)
        {
            flight.Id = _id++;
            _flights.Add(flight);
        }

        public static void DeleteFlightById(int id)
        {
            var flightToRemove = GetFlightById(id);

            if (flightToRemove != null)
            {
                _flights.Remove(flightToRemove);
            }
        }

        public static Flight? GetFlightById(int id)
        {
            return _flights.FirstOrDefault(flight => flight.Id == id);
        }

        public static void Clear()
        {
            _flights.Clear();
        }

        public static bool IsFlightValid(Flight flight)
        {
            if (flight.From.AirportCode.ToLower().Trim() == flight.To.AirportCode.ToLower().Trim())
            {
                return false;
            }

            DateTime? arrivalDate = GetDateTimeFromString(flight.ArrivalTime);
            DateTime? departureDate = GetDateTimeFromString(flight.DepartureTime);

            if (arrivalDate == null || departureDate == null || departureDate >= arrivalDate)
            {
                return false;
            }

            return true;
        }

        public static DateTime? GetDateTimeFromString(string date)
        {
            if (DateTime.TryParseExact(date, "yyyy-MM-dd HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime result))
            {
                return result;
            }

            return null;

        }

        public static bool FlightExists(Flight flight)
        {
            return _flights.Any(f => f.Carrier.ToLower().Trim() == flight.Carrier.ToLower().Trim()
                                     && f.DepartureTime == flight.DepartureTime &&
                                     f.ArrivalTime == flight.ArrivalTime
                                     && f.From.AirportCode.ToLower().Trim() ==
                                     flight.From.AirportCode.ToLower().Trim()
                                     && flight.To.AirportCode.ToLower().Trim() ==
                                     flight.To.AirportCode.ToLower().Trim());
        }

        public static List<Airport> SearchAirports(string keyword)
        {
            keyword = keyword.ToLower().Trim();

            List<Airport> airports = GetAllAirports().ToList();
            List<Airport> resultAirports = new();

            foreach (Airport airport in airports)
            {
                if (airport.City.ToLower().Contains(keyword) || airport.Country.ToLower().Contains(keyword) || airport.AirportCode.ToLower() == keyword)
                {
                    resultAirports.Add(airport);
                }
            }

            return resultAirports;
        }

        public static HashSet<Airport> GetAllAirports()
        {
            HashSet<Airport> airports = new();

            foreach (var flight in _flights)
            {
                airports.Add(flight.From);
                airports.Add(flight.To);
            }

            return airports;
        }
    }
}
