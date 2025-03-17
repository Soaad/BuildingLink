using System.Data;
using Dapper;
using Task_BuildingLink.Domain.Contracts;
using Task_BuildingLink.Domain.Models;

namespace Task_BuildingLink.Infrastructure.Repositories;

public class DriverRepository : IDriverRepository
{
    private readonly IDbConnection _dbConnection;

    public DriverRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
        DatabaseInitializer.Initialize(_dbConnection);
    }

    public IEnumerable<Driver> GetAllDrivers() =>
        _dbConnection.Query<Driver>("SELECT * FROM Drivers ORDER BY FirstName");

    public Driver? GetDriverById(int id) =>
        _dbConnection.QueryFirstOrDefault<Driver>("SELECT * FROM Drivers WHERE Id = @id", new { id });

    public void AddDriver(Driver driver) =>
        _dbConnection.Execute(
            "INSERT INTO Drivers (FirstName, LastName, Email, PhoneNumber) VALUES (@FirstName, @LastName, @Email, @PhoneNumber)",
            driver);

    public bool UpdateDriver(int id, Driver driver)
    {
        int rowsAffected = _dbConnection.Execute(
            "UPDATE Drivers SET FirstName = @FirstName, LastName = @LastName, Email = @Email, PhoneNumber = @PhoneNumber WHERE Id = @id",
            new { id, driver.FirstName, driver.LastName, driver.Email, driver.PhoneNumber });

        return rowsAffected > 0;
    }

    public bool DeleteDriver(int id)
    {
        int rowsAffected = _dbConnection.Execute("DELETE FROM Drivers WHERE Id = @id", new { id });
        return rowsAffected > 0;
    }

    public void SeedData()
    {
        var names = new[] { "Alice", "Bob", "Charlie", "David", "Eve", "Frank", "Grace", "Hank", "Ivy", "Jack" };
        foreach (var name in names)
        {
            _dbConnection.Execute(
                "INSERT INTO Drivers (FirstName, LastName, Email, PhoneNumber) VALUES (@FirstName, 'Random', @Email, '123456789')",
                new { FirstName = name, Email = $"{name.ToLower()}@example.com" });
        }
    }

    public string AlphabetizeDriverName(int id)
    {
        var driver = GetDriverById(id);
        return driver != null ? string.Concat((driver.FirstName + " " + driver.LastName)) : "";
    }

    public List<string> AlphabetizeDriverNames()
    {
        var names = new List<string>();
        foreach (var driver in GetAllDrivers())
        {
            names.Add(driver != null ? string.Concat((driver.FirstName + " " + driver.LastName)) : "");
        }

        return names;
    }
}