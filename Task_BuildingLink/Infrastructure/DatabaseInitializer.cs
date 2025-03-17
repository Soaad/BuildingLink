using System.Data;
using Dapper;

namespace Task_BuildingLink.Infrastructure;

public class DatabaseInitializer
{
    public static void Initialize(IDbConnection connection)
    {
        connection.Open(); // Ensure connection is open
        const string createTableQuery = @"
            CREATE TABLE IF NOT EXISTS Drivers (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                FirstName TEXT NOT NULL,
                LastName TEXT NOT NULL,
                Email TEXT NOT NULL UNIQUE,
                PhoneNumber TEXT NOT NULL
            );";

        connection.Execute(createTableQuery);
    }
}