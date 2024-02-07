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

    public async Task<FlightPlan> GetFlightPlansById(string flightPlanId)
    {
        IMongoCollection<BsonDocument> collection = GetCollection("pluralsight", "flight_plans");
        IAsyncCursor<BsonDocument> flightPlanCursors = await collection.FindAsync(
            Builders<BsonDocument>.Filter.Eq("flight_plan_id", flightPlanId));

        BsonDocument document = flightPlanCursors.FirstOrDefault();

        FlightPlan flightPlan = ConvertBsonToFlightPlan(document);

        if (flightPlan == null)
        {
            return new FlightPlan();
        }

        return flightPlan;
    }

    public async Task<bool> FileFlightPlan(FlightPlan flightPlan)
    {
        IMongoCollection<BsonDocument> collection = GetCollection("pluralsight", "flight_plans");

        var document = new BsonDocument
        {
            {"flight_plan_id", Guid. NewGuid().ToString("N") },
            {"altitude", flightPlan.Altitude },
            {"airspeed", flightPlan.Airspeed },
            {"aircraft_identification", flightPlan.AircraftIdentification },
            {"aircraft_type", flightPlan.AircraftType },
            {"arrival_airport", flightPlan.ArrivalAirport },
            {"flight_type", flightPlan.FlightType},
            {"departing_airport", flightPlan.DepartingAirport },
            {"departure_time", flightPlan.DepartureTime },
            {"estimated_arrival_time", flightPlan.EstimatedArrivalTime }, {"route", flightPlan. Route },
            {"remarks", flightPlan.Remarks },
            {"fuel_hours", flightPlan.FuelHours },
            {"fuel_minutes", flightPlan.FuelMinutes },
            {"number_onboard", flightPlan.NumberOnboard }
        };

        try
        {
            await collection.InsertOneAsync(document);
        }
        catch
        {

            return false;
        }

        return true;
    }

    public async Task<bool> UpdateFlightPlan(string flightPlanId, FlightPlan flightPlan)
    {

        IMongoCollection<BsonDocument> collection = GetCollection("pluralsight", "flight_plans");
        FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("pluralsight", flightPlanId);

        UpdateDefinition<BsonDocument> update = Builders<BsonDocument>.Update
            .Set("altitude", flightPlan.Altitude)
            .Set("airspeed", flightPlan.Airspeed)
            .Set("aircraft_identification", flightPlan.AircraftIdentification)
            .Set("aircraft_type", flightPlan.AircraftType)
            .Set("arrival_airport", flightPlan.ArrivalAirport)
            .Set("flight_type", flightPlan.FlightType)
            .Set("departing_airport", flightPlan.DepartingAirport)
            .Set("departure_time", flightPlan.DepartureTime)
            .Set("estimated_arrival_time", flightPlan.EstimatedArrivalTime)
            .Set("route", flightPlan.Route)
            .Set("remarks", flightPlan.Remarks)
            .Set("fuel_hours", flightPlan.FuelHours)
            .Set("fuel_minutes", flightPlan.FuelMinutes)
            .Set("numberOnBoard", flightPlan.NumberOnboard);

        UpdateResult result = await collection.UpdateOneAsync(filter, update);

        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteFlightPlanById(string flightPlanId)
    {
        IMongoCollection<BsonDocument> collection = GetCollection("pluralsight", "flight_plans");
        DeleteResult result = await collection.DeleteOneAsync(Builders<BsonDocument>.Filter.Eq("pluralsight", flightPlanId));

        return result.DeletedCount > 0;
    }
}
