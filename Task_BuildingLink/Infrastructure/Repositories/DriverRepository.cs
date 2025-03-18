using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;
using Task_BuildingLink.Domain.Contracts;
using Task_BuildingLink.Domain.Models;

namespace Task_BuildingLink.Infrastructure.Repositories;

public class DriverRepository : IDriverRepository
{
    private readonly IDbConnection _dbConnection;
    private readonly ILogger <DriverRepository>_logger;
    public DriverRepository(IDbConnection dbConnection, ILogger<DriverRepository> logger)
    {
        _dbConnection = dbConnection;
        DatabaseInitializer.Initialize(_dbConnection);
        _logger = logger;   
    }

    public IEnumerable<Driver> GetAllDrivers()
    {
        try
        {
          return _dbConnection.Query<Driver>("SELECT * FROM Drivers ORDER BY FirstName");

        }
        catch (SqliteException ex)
        {
            _logger.LogError(ex, "Database error occurred while retrieving drivers.");
            throw new Exception("Database error. Please try again later.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred.");
            throw;
        }
    }

    public Driver? GetDriverById(int id)
    {
        try
        {
            var query = "SELECT * FROM Drivers WHERE Id = @Id";
            return _dbConnection.QueryFirstOrDefault<Driver>(query, new { Id = id });
        }
        catch (SqliteException ex)
        {
            _logger.LogError(ex, "Database error occurred while retrieving driver.");
            throw new Exception("Database error. Please try again later.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred.");
            throw;
        }
    }

    public void AddDriver(Driver driver)
    {
        try
        {
            _dbConnection.Execute(
                "INSERT INTO Drivers (FirstName, LastName, Email, PhoneNumber) VALUES (@FirstName, @LastName, @Email, @PhoneNumber)",
                driver);
        }
        catch (SqliteException ex)
        {
            _logger.LogError(ex, "Database error occurred while adding driver.");
            throw new Exception("Database error. Please try again later.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred.");
            throw;
        }
        

    }
        
    public bool UpdateDriver(int id, Driver driver)
    {
        try
        {
            int rowsAffected = _dbConnection.Execute(
                "UPDATE Drivers SET FirstName = @FirstName, LastName = @LastName, Email = @Email, PhoneNumber = @PhoneNumber WHERE Id = @id",
                new { id, driver.FirstName, driver.LastName, driver.Email, driver.PhoneNumber });

            return rowsAffected > 0;
        }
        catch (SqliteException ex)
        {
            _logger.LogError(ex, "Database error occurred while updating driver.");
            throw new Exception("Database error. Please try again later.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred.");
            throw;
        }
        
    }

    public bool DeleteDriver(int id)
    {
        try
        { 
            int rowsAffected = _dbConnection.Execute("DELETE FROM Drivers WHERE Id = @id", new { id });
            return rowsAffected > 0;
        }
        catch (SqliteException ex)
        {
            _logger.LogError(ex, "Database error occurred while deleting driver.");
            throw new Exception("Database error. Please try again later.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred.");
            throw;
        }
      
    }

    public void SeedData()
    {
        try
        {
            var names = new[] { "Alice", "Bob", "Charlie", "David", "Eve", "Frank", "Grace", "Hank", "Ivy", "Jack" };
            foreach (var name in names)
            {
                _dbConnection.Execute(
                    "INSERT INTO Drivers (FirstName, LastName, Email, PhoneNumber) VALUES (@FirstName, 'Random', @Email, '123456789')",
                    new { FirstName = name, Email = $"{name.ToLower()}@example.com" });
            }
        }
        catch (SqliteException ex)
        {
            _logger.LogError(ex, "Database error occurred while seeding data.");
            throw new Exception("Database error. Please try again later.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred.");
            throw;
        }
       
    }

    public string AlphabetizeDriverName(int id)
    {
        try
        {
            var driver = GetDriverById(id);
            string firstName = new string(driver.FirstName.OrderBy(c => c).ToArray());
            string lastName = new string(driver.LastName.OrderBy(c => c).ToArray());    
            return driver != null ? string.Concat(firstName+ " " +lastName ) : "";
        }

        catch (SqliteException ex)
        {
            _logger.LogError(ex, "Database error occurred while AlphabetizeDriverName.");
            throw new Exception("Database error. Please try again later.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred.");
            throw;
        }

    }

    public List<string> AlphabetizeDriverNames()
    {
        try
        {
            return GetAllDrivers()
                .Select(driver => $"{driver.FirstName} {driver.LastName}") // Get full name
                .OrderBy(name => name) // Sort alphabetically
                .ToList();
        }
        catch (SqliteException ex)
        {
            _logger.LogError(ex, "Database error occurred while AlphabetizeDriverName.");
            throw new Exception("Database error. Please try again later.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred.");
            throw;
        }
        
    }
}