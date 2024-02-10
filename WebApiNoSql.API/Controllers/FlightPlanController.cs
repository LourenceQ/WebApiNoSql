using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using WebApiNoSql.API.Data;
using WebApiNoSql.API.Models;

namespace WebApiNoSql.API.Controllers;
[Route("api/v1/flightplan")]
[ApiController]
public class FlightPlanController(IDataBaseAdapter database) : ControllerBase
{
    private readonly IDataBaseAdapter _database = database;


    [HttpGet]
    [Authorize]
    [SwaggerResponse((int)HttpStatusCode.NoContent, "No flight plans have been filed with this system")]
    public async Task<IActionResult> GetFlightPlanList()
    {
        List<FlightPlan> flightPlanList = await _database.GetAllFlightPlans();

        return flightPlanList.Count != 0 ? Ok(flightPlanList) : NoContent();
    }

    [HttpGet("{flightPlanId}")]
    [Authorize]
    public async Task<IActionResult> GetFlightPlanById(string flightPlanId)
    {
        FlightPlan flightPlan = await _database.GetFlightPlansById(flightPlanId);

        return flightPlan.FlightPlanId == flightPlanId ? Ok(flightPlan) : StatusCode(StatusCodes.Status404NotFound);
    }


    /// <summary>
    /// Files a new flight plan with the system
    /// </summary>
    /// <param name="flightPlan"><see cref="FlightPlan"/></param>
    /// <remarks>
    ///     Sample request:
    ///     
    ///         POST / api/v1/flightplan/file
    ///         {
    ///             "flight_plan_id": "string",
    ///             "altitude": 0,
    ///             "airspeed": 0,
    ///             "aircraft_identification": "string",
    ///             "aircraft_type": "string",
    ///             "arrival_airport": "string",
    ///             "departing_airport": "string",
    ///             "flight_type": "string",
    ///             "departure_time": "2024-02-16T00:07:03.792Z",
    ///             "estimated_arrival_time": "2024-02-16T00:07:03.792Z",
    ///             "route": "string",
    ///             "remarks": "string",
    ///             "fuel_hours": 0,
    ///             "fuel_minutes": 0,
    ///             "number_onboard": 0
    ///         }
    /// </remarks>    
    /// <returns><see cref="Task{IActionResult}"/></returns>
    [HttpPost("file")]
    [Authorize]
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
    [Authorize]
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
    [Authorize]
    public async Task<IActionResult> DeleteFlightPlan(string flightPlanId)
    {
        bool result = await _database.DeleteFlightPlanById(flightPlanId);

        return result ? Ok() : StatusCode(StatusCodes.Status404NotFound);
    }

    [HttpGet("airport/departure/{flightPlanId}")]
    [Authorize]
    public async Task<IActionResult> GetFlightPlanDepartureAirport(string flightPlanId)
    {
        FlightPlan flightPlan = await _database.GetFlightPlansById(flightPlanId);

        return flightPlan == null ? StatusCode(StatusCodes.Status404NotFound) : Ok(flightPlan.DepartingAirport);
    }

    [HttpGet("route/{flightPlanId}")]
    [Authorize]
    public async Task<IActionResult> GetFlightPlanRoutes(string flightPlanId)
    {
        FlightPlan flightPlan = await _database.GetFlightPlansById(flightPlanId);

        return flightPlan == null ? StatusCode(StatusCodes.Status404NotFound) : Ok(flightPlan.Route);
    }

    [Authorize]
    [HttpGet("time/enroute/{flightPlanId}")]
    public async Task<IActionResult> GetFlightPlanTimeEnroute(string flightPlanId)
    {
        FlightPlan flightPlan = await _database.GetFlightPlansById(flightPlanId);

        return flightPlan == null ? StatusCode(StatusCodes.Status404NotFound) : Ok(flightPlan.EstimatedArrivalTime - flightPlan.DepartureTime);
    }

}
