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
        throw new NotImplementedException();
    }

    [HttpGet]
    public async Task<IActionResult> GetFlightPlanById(string flightPlanId)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public async Task<IActionResult> FileFlightPlan(FlightPlan flightPlan)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public async Task<IActionResult> UpdateFlightPlan(FlightPlan flightPlan)
    {
        throw new NotImplementedException();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteFlightPlan(FlightPlan flightPlanId)
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    public async Task<IActionResult> GetFlightPlanDepartureAirport(string flightPlanId)
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    public async Task<IActionResult> GetFlightPlanRoutes(string flightPlanId)
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    public async Task<IActionResult> GetFlightPlanTimeEnroute(string flightPlanId)
    {
        throw new NotImplementedException();
    }



}
