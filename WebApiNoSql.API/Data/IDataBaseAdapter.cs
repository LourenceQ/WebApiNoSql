using WebApiNoSql.API.Models;

namespace WebApiNoSql.API.Data;

public interface IDataBaseAdapter
{
    Task<List<FlightPlan>> GetAllFlightPlans();
    Task<List<FlightPlan>> GetFlightPlansById(string flightPlanId);
    Task<bool> FileFlightPlan(FlightPlan flightPlan);
    Task<bool> UpdateFlightPlan(string flightPlanId, FlightPlan flightPlan);
    Task<bool> DeleteFlightPlanById(string flightPlanId);
}
