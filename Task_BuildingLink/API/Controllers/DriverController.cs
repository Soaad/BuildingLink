using Microsoft.AspNetCore.Mvc;
using Task_BuildingLink.Domain.Contracts;
using Task_BuildingLink.Domain.Models;
using Task_BuildingLink.Middlewares.Custom_Exceptions;

namespace Task_BuildingLink.API.Controllers;

[Route("api/[controller]")]
public class DriverController : ControllerBase
{
    private readonly IDriverService _driverService;
    private readonly ILogger<DriverController> _logger;
    public DriverController(IDriverService driverService,ILogger<DriverController> logger)
    {
            _driverService = driverService;
            _logger = logger;
    }
    [HttpGet]
    public ActionResult<IEnumerable<Driver>> GetDrivers()
    {
        try
        {
            return Ok(_driverService.GetDrivers());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching drivers.");
            return StatusCode(500, new { message = "An unexpected error occurred." });
        }
       
    }

    [HttpGet("{id}")]
    public ActionResult<Driver> GetDriver(int id)
    {
        try
        {
            var driver = _driverService.GetDriver(id);
            return Ok(driver);

        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching driver.");
            return StatusCode(500, new { message = "An unexpected error occurred." });
        }
       
    }

    [HttpPost]
    public IActionResult CreateDriver([FromBody] Driver driver)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _driverService.CreateDriver(driver);
            return CreatedAtAction(nameof(GetDriver), new { id = driver.Id }, driver);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating driver .");
            return StatusCode(500, new { message = "An unexpected error occurred." });
        }
       
    }

    [HttpPut("{id}")]
    public IActionResult UpdateDriver(int id, [FromBody] Driver driver)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_driverService.UpdateDriver(id, driver))
            {
                return NoContent();
            }
            return NotFound(new { message = $"Driver with ID {id} not found." });
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex.Message);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating driver with ID {id}.");
            return StatusCode(500, new { message = "An error occurred while updating the driver." });
        }

    }

    [HttpDelete("{id}")]
    public IActionResult DeleteDriver(int id)
    {
        try
        {
         if(_driverService.DeleteDriver(id))
             return NoContent();
         return NotFound(new { message = $"Driver with ID {id} not found." });  
        }
        
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex.Message);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting driver with ID {id}.");
            return StatusCode(500, new { message = "An error occurred while deleting the driver." });
        }
    }
        

    [HttpPost("seed")]
    public IActionResult SeedData()
    {
        try
        {
            _driverService.SeedData();
            return Ok(new { message = "Data seeded successfully,10 random drivers have been added successfully." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error seeding data.");
            return StatusCode(500, new { message = "An error occurred while seeding data." });
        }
    }

    [HttpGet("alphabetize/{id}")]
    public IActionResult AlphabetizeDriver(int id)
    {
        try
        {
            var result = _driverService.AlphabetizeDriver(id);
            return Ok(new { message = $"Alphabetized name: {result}" });
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex.Message);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error alphabetizing driver with ID {id}.");
            return StatusCode(500, new { message = "An error occurred while alphabetizing the driver's name." });
        }
    }

   
   [HttpGet("alphabetize")]
   public ActionResult<List<string>> AlphabetizeDriverNames()
   {
       try
       {
           var alphabetizedNames = _driverService.AlphabetizeDriverNames(); 
           return  Ok(alphabetizedNames);
       }
        
       catch (Exception ex)
       {
           _logger.LogError(ex, $"Error alphabetizing drivers.");
           return StatusCode(500, new { message = "An error occurred while alphabetizing  drivers names." });
       }
       
   }
}