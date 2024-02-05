using System.Text.Json.Serialization;

namespace WebApiNoSql.API.Models;

public class FlightPlan
{
    [JsonPropertyName("flight_plan_id")]
    public string? FlightPlanId { get; set; }

    [JsonPropertyName("altitude")]
    public int Altitude { get; set; }

    [JsonPropertyName("airspeed")]
    public int Airspeed { get; set; }

    [JsonPropertyName("aircraft_identification")]
    public string? AircraftIdentification { get; set; }

    [JsonPropertyName("aircraft_type")]
    public string? AircraftType { get; set; }

    [JsonPropertyName("arrival_airport")]
    public string? ArrivalAirport { get; set; }

    [JsonPropertyName("departing_airport")]
    public string? DepartingAirport { get; set; }

    [JsonPropertyName("flight_type")]
    public string? FlightType { get; set; }

    [JsonPropertyName("departure_time")]
    public DepartureTime? DepartureTime { get; set; }

    [JsonPropertyName("estimated_arrival_time")]
    public EstimatedArrivalTime? EstimatedArrivalTime { get; set; }

    [JsonPropertyName("route")]
    public string? Route { get; set; }

    [JsonPropertyName("remarks")]
    public string? Remarks { get; set; }

    [JsonPropertyName("fuel_hours")]
    public int FuelHours { get; set; }

    [JsonPropertyName("fuel_minutes")]
    public int FuelMinutes { get; set; }

    [JsonPropertyName("number_onboard")]
    public int NumberOnboard { get; set; }

}

public class DepartureTime
{
    [JsonPropertyName("date")]
    public DateTime? Date { get; set; }
}

public class EstimatedArrivalTime
{
    [JsonPropertyName("date")]
    public DateTime? Date { get; set; }
}



