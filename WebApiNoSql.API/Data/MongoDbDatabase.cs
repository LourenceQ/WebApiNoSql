using MongoDB.Bson;
using MongoDB.Driver;
using WebApiNoSql.API.Models;

namespace WebApiNoSql.API.Data;

public class MongoDbDatabase : IDataBaseAdapter
{
    private IMongoCollection<BsonDocument> GetCollection(string databaseName, string collectionName)
    {
        MongoClient client = new();
        IMongoDatabase database = client.GetDatabase(databaseName);
        IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>(collectionName);

        return collection;
    }

    private FlightPlan ConvertBsonToFlightPlan(BsonDocument document)
    {
        if (document == null) return null;

        return new FlightPlan
        {
            FlightPlanId = document["flight_plan_id"].AsString,
            Altitude = document["altitude"].AsInt32,
            Airspeed = document["airspeed"].AsInt32,
            AircraftIdentification = document["aircraft_identification"].AsString,
            AircraftType = document["aircraft_type"].AsString,
            ArrivalAirport = document["arrival_airport"].AsString,
            DepartingAirport = document["departing_airport"].AsString,
            FlightType = document["flight_type"].AsString,
            DepartureTime = document["departure_time"].AsBsonDateTime.ToUniversalTime(),
            EstimatedArrivalTime = document["estimated_arrival_time"].AsBsonDateTime.ToUniversalTime(),
            Route = document["route"].AsString,
            Remarks = document["remarks"].AsString,
            FuelHours = document["fuel_hours"].AsInt32,
            FuelMinutes = document["fuel_minutes"].AsInt32,
            NumberOnboard = document["number_onboard"].AsInt32
        };
    }

    public async Task<List<FlightPlan>> GetAllFlightPlans()
    {
        IMongoCollection<BsonDocument> collection = GetCollection("pluralsight", "flight_plans");
        Task<List<BsonDocument>> documents = collection.Find(_ => true).ToListAsync();

        List<FlightPlan> flightPlanList = [];

        if (documents == null) return null;

        foreach (var document in await documents)
        {
            flightPlanList.Add(ConvertBsonToFlightPlan(document));
        }

        return flightPlanList;
    }

    public Task<List<FlightPlan>> GetFlightPlansById(string flightPlanId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> FileFlightPlan(FlightPlan flightPlan)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateFlightPlan(string flightPlanId, FlightPlan flightPlan)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteFlightPlanById(string flightPlanId)
    {
        throw new NotImplementedException();
    }
}
