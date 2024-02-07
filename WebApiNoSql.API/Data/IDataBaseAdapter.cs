using WebApiNoSql.API.Models;

namespace WebApiNoSql.API.Data;

public interface IDataBaseAdapter
{
    Task<List<FlightPlan>> GetAllFlightPlans();
    Task<FlightPlan> GetFlightPlansById(string flightPlanId);
    Task<TransactionResult> FileFlightPlan(FlightPlan flightPlan);
    Task<TransactionResult> UpdateFlightPlan(string flightPlanId, FlightPlan flightPlan);
    Task<bool> DeleteFlightPlanById(string flightPlanId);
}
