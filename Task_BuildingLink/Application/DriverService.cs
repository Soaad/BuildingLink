using Task_BuildingLink.Domain.Contracts;
using Task_BuildingLink.Domain.Models;
using Task_BuildingLink.Middlewares.Custom_Exceptions;

namespace Task_BuildingLink.Application;

public class DriverService:IDriverService   
{
    IDriverRepository _repository;
    private readonly ILogger <DriverService>_logger;
    public DriverService(IDriverRepository repository, ILogger <DriverService> logger)
    {
        _repository = repository;   
        _logger = logger;
            
    }
    public IEnumerable<Driver> GetDrivers()
    {
        try
        {
            return _repository.GetAllDrivers(); 

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in DriverService.");
            throw new Exception("An error occurred while retrieving  drivers.");
        }
        
    }

    public Driver? GetDriver(int id)
    {
        try
        {
            var driver =  _repository.GetDriverById(id);
            if (driver == null)
            {
                throw new NotFoundException($"Driver with ID {id} not found.");
            }
            return driver;
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in DriverService.");
            throw new Exception("An error occurred while retrieving the driver.");
        }
    }

    public void CreateDriver(Driver driver)
    {
        try
        {
            _repository.AddDriver(driver);  
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating driver.");
            throw new Exception("An error occurred while creating the driver.");
        }
          
    }

    public bool UpdateDriver(int id, Driver driver)
    {
        try
        {
            if (!_repository.UpdateDriver(id, driver))
            {
                throw new NotFoundException($"Driver with ID {id} not found.");
            }

            return true;
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating driver with ID {id}.");
            throw new Exception("An error occurred while updating the driver.");
        }

    }

    public bool DeleteDriver(int id)
    {
       
       try
       {
           if (!_repository.DeleteDriver(id))
           {
               throw new NotFoundException($"Driver with ID {id} not found.");
           }
           return true;
       }
       catch (NotFoundException)
       {
           throw;
       }
       catch (Exception ex)
       {
           _logger.LogError(ex, $"Error deleting driver with ID {id}.");
           throw new Exception("An error occurred while deleting the driver.");
       }
    }

    public void SeedData()
    {
       
       try
       {
           _repository.SeedData();

       }
       catch (Exception ex)
       {
           _logger.LogError(ex, "Error seeding driver data.");
           throw new Exception("An error occurred while seeding driver data.");
       }
    }

    public string AlphabetizeDriver(int id)
    {
        try
        {
            var result = _repository.AlphabetizeDriverName(id);
            if (string.IsNullOrEmpty(result))
            {
                throw new NotFoundException($"Driver with ID {id} not found.");
            }
            return result;
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error alphabetizing driver with ID {id}.");
            throw new Exception("An error occurred while alphabetizing the driver's name.");
        }
    }

    public List<string> AlphabetizeDriverNames()
    {
        try
        {
            return _repository.AlphabetizeDriverNames();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error alphabetizing driver names.");
            throw new Exception("An error occurred while alphabetizing driver names.");
        }    
    }
}