using Microsoft.AspNetCore.Mvc;
using Task_BuildingLink.Domain.Contracts;
using Task_BuildingLink.Domain.Models;

namespace Task_BuildingLink.API.Controllers;

[Route("api/[controller]")]
public class DriverController : ControllerBase
{
    private readonly IDriverService _driverService;

    public DriverController(IDriverService driverService)
    {
            _driverService = driverService; 
    }
    [HttpGet]
    public ActionResult<IEnumerable<Driver>> GetDrivers() => Ok(_driverService.GetDrivers());

    [HttpGet("{id}")]
    public ActionResult<Driver> GetDriver(int id)
    {
        var driver = _driverService.GetDriver(id);
        return driver != null ? Ok(driver) : NotFound();
    }

    [HttpPost]
    public IActionResult CreateDriver(Driver driver)
    {
        _driverService.CreateDriver(driver);
        return CreatedAtAction(nameof(GetDriver), new { id = driver.Id }, driver);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateDriver(int id, Driver driver) =>
        _driverService.UpdateDriver(id, driver) ? NoContent() : NotFound();

    [HttpDelete("{id}")]
    public IActionResult DeleteDriver(int id) =>
        _driverService.DeleteDriver(id) ? NoContent() : NotFound();

    [HttpPost("seed")]
    public IActionResult SeedData()
    {
        _driverService.SeedData();
        return Ok("10 random drivers added.");
    }

    [HttpGet("alphabetize/{id}")]
    public IActionResult AlphabetizeDriver(int id)
    {
        var alphabetizedName = _driverService.AlphabetizeDriver(id);
        return string.IsNullOrEmpty(alphabetizedName) ? NotFound() : Ok(alphabetizedName);
    }

   
   [HttpGet("alphabetize")]
   public ActionResult<List<string>> AlphabetizeDriverNames()
   {
       var alphabetizedNames = _driverService.AlphabetizeDriverNames(); 
       return alphabetizedNames == null ? NotFound() : Ok(alphabetizedNames);
   }
}