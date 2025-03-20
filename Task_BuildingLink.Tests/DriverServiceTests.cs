using Microsoft.Extensions.Logging;
using Moq;
using Task_BuildingLink.Application;
using Task_BuildingLink.Domain.Contracts;
using Task_BuildingLink.Domain.Models;
using Task_BuildingLink.Middlewares.Custom_Exceptions;

namespace Task_BuildingLink.Tests;

public class DriverServiceTests
{
    private readonly DriverService _driverService;
    private readonly Mock<IDriverRepository> _repositoryMock = new Mock<IDriverRepository>();
    private readonly Mock<ILogger<DriverService>> _loggerMock = new Mock<ILogger<DriverService>>();
    public DriverServiceTests()
    {
        _driverService = new DriverService(_repositoryMock.Object, _loggerMock.Object);
    }
    [Fact]
    public void GetDrivers_ShouldReturnDrivers()
    {
        // Arrange
        var expectedDrivers = new List<Driver>
        {
            new Driver { Id = 1, FirstName = "Alice", LastName = "Doe", Email = "alice@example.com", PhoneNumber = "1234567890"  }
        };
        _repositoryMock.Setup(repo => repo.GetAllDrivers()).Returns(expectedDrivers);

        // Act
        var result = _driverService.GetDrivers();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Alice", result.First().FirstName);
    }


    [Fact]
    public void GetDriver_withVaildId_ShouldReturnDriver()
    {
        // Arrange
        var driver = new Driver
        {
            Id = 1, FirstName = "Alice", LastName = "Doe", Email = "alice@example.com", PhoneNumber = "1234567890"
        };
        _repositoryMock.Setup(repo => repo.GetDriverById(1)).Returns(driver);

        // Act
        var result = _driverService.GetDriver(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Alice", result.FirstName);
    }
    [Fact]
    public void GetDriver_WithInvalidId_ShouldThrowNotFoundException()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.GetDriverById(99)).Returns((Driver)null);

        // Act & Assert
        var exception = Assert.Throws<NotFoundException>(() => _driverService.GetDriver(99));
        Assert.Equal("Driver with ID 99 not found.", exception.Message);
    }
    
    [Fact]
    public void AlphabetizeDriverNames_ShouldReturnSortedNames()
    {
        // Arrange
        var drivers = new List<string> { "Charlie John", "Alice Harry", "Bob Doe" };
        _repositoryMock.Setup(repo => repo.AlphabetizeDriverNames()).Returns(drivers);

        // Act
        var result = _driverService.AlphabetizeDriverNames();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal(drivers, result); // Ensures the returned list is correct


        _repositoryMock.Verify(repo => repo.AlphabetizeDriverNames(), Times.Once);
    }
}