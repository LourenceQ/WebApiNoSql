using Microsoft.AspNetCore.Mvc;
using WebApiNoSql.API.Data;
using WebApiNoSql.API.Models;

namespace WebApiNoSql.API.Controllers;
[Route("api/v1/flightplan")]
[ApiController]
public class FlightPlanController(IDataBaseAdapter database) : ControllerBase
{
    private readonly IDataBaseAdapter _database = database;

    [HttpGet]
    public async Task<IActionResult> GetFlightPlanList()
    {
        List<FlightPlan> flightPlanList = await _database.GetAllFlightPlans();

        return flightPlanList.Count != 0 ? Ok(flightPlanList) : NoContent();
    }

    [HttpGet("{flightPlanId}")]
    public async Task<IActionResult> GetFlightPlanById(string flightPlanId)
    {
        FlightPlan flightPlan = await _database.GetFlightPlansById(flightPlanId);

        return flightPlan.FlightPlanId == flightPlanId ? Ok(flightPlan) : StatusCode(StatusCodes.Status404NotFound);
    }

    [HttpPost("file")]
    public async Task<IActionResult> FileFlightPlan(FlightPlan flightPlan)
    {
        TransactionResult transactionResult = await _database.FileFlightPlan(flightPlan);

        return transactionResult switch
        {
            TransactionResult.Success => Ok(),
            TransactionResult.BadRequest => StatusCode(StatusCodes.Status400BadRequest),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }

    [HttpPut]
    public async Task<IActionResult> UpdateFlightPlan(FlightPlan flightPlan)
    {
        TransactionResult updateResult = await _database.UpdateFlightPlan(flightPlan.FlightPlanId, flightPlan);

        return updateResult switch
        {
            TransactionResult.Success => Ok(),
            TransactionResult.NotFound => StatusCode(StatusCodes.Status400BadRequest),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }

    [HttpDelete("{flightPlanId}")]
    public async Task<IActionResult> DeleteFlightPlan(string flightPlanId)
    {
        bool result = await _database.DeleteFlightPlanById(flightPlanId);

        return result ? Ok() : StatusCode(StatusCodes.Status404NotFound);
    }

    [HttpGet("airport/departure/{flightPlanId}")]
    public async Task<IActionResult> GetFlightPlanDepartureAirport(string flightPlanId)
    {
        FlightPlan flightPlan = await _database.GetFlightPlansById(flightPlanId);

        return flightPlan == null ? StatusCode(StatusCodes.Status404NotFound) : Ok(flightPlan.DepartingAirport);
    }

    [HttpGet("route/{flightPlanId}")]
    public async Task<IActionResult> GetFlightPlanRoutes(string flightPlanId)
    {
        FlightPlan flightPlan = await _database.GetFlightPlansById(flightPlanId);

        return flightPlan == null ? StatusCode(StatusCodes.Status404NotFound) : Ok(flightPlan.Route);
    }

    [HttpGet("time/enroute/{flightPlanId}")]
    public async Task<IActionResult> GetFlightPlanTimeEnroute(string flightPlanId)
    {
        FlightPlan flightPlan = await _database.GetFlightPlansById(flightPlanId);

        return flightPlan == null ? StatusCode(StatusCodes.Status404NotFound) : Ok(flightPlan.ArrivalAirport - flightPlan.DepartureTime);
    }



}
