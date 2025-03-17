using Task_BuildingLink.Domain.Models;

namespace Task_BuildingLink.Domain.Contracts;

public interface IDriverRepository
{
     IEnumerable<Driver> GetAllDrivers();

     Driver? GetDriverById(int id) ;

     void AddDriver(Driver driver);
     bool UpdateDriver(int id, Driver driver);
     
     bool DeleteDriver(int id);
     string AlphabetizeDriverName(int id);
     void SeedData();
     List<string> AlphabetizeDriverNames();
}