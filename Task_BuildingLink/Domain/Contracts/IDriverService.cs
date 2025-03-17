using Task_BuildingLink.Domain.Models;

namespace Task_BuildingLink.Domain.Contracts;

public interface IDriverService
{
    IEnumerable<Driver> GetDrivers();
    Driver? GetDriver(int id);
    void CreateDriver(Driver driver);
    bool UpdateDriver(int id, Driver driver);
    bool DeleteDriver(int id);
    void SeedData();
    string AlphabetizeDriver(int id);
    List<string> AlphabetizeDriverNames();
}