using Task_BuildingLink.Domain.Contracts;
using Task_BuildingLink.Domain.Models;

namespace Task_BuildingLink.Application;

public class DriverService:IDriverService   
{
    IDriverRepository _repository;

    public DriverService(IDriverRepository repository)
    {
        _repository = repository;   
            
    }
    public IEnumerable<Driver> GetDrivers()
    {
        return _repository.GetAllDrivers(); 
    }

    public Driver? GetDriver(int id)
    {
        return _repository.GetDriverById(id);
    }

    public void CreateDriver(Driver driver)
    {
         _repository.AddDriver(driver);   
    }

    public bool UpdateDriver(int id, Driver driver)
    {
      return  _repository.UpdateDriver(id, driver);   
    }

    public bool DeleteDriver(int id)
    {
       return _repository.DeleteDriver(id);    
    }

    public void SeedData()
    {
       _repository.SeedData();
    }

    public string AlphabetizeDriver(int id)
    {
       return _repository.AlphabetizeDriverName(id);
    }

    public List<string> AlphabetizeDriverNames()
    {
        return _repository.AlphabetizeDriverNames();    
    }
}