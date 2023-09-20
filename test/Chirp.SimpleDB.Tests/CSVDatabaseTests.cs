namespace Chirp.SimpleDB.Tests;

using SimpleDB;

public class CSVDatabaseTests
{
    [Fact]
    public void StoreAndReadCheepsTest() {
        // Arrange
        var database = CSVDatabase.CSVDatabase<Program.Cheep>.Instance;
        database.filename = Environment.CurrentDirectory + @"../../../../testDatabase.csv";
        var cheep = new Program.Cheep("testUser", "testCheep", 1690981487); // unixtime =  02.08.2023 15.04.47

        // Act
        //database.Store(cheep); // commented out until we can delete cheeps from the test database
        var readCheeps = database.Read(1);

        // Assert
        Assert.Equal(cheep, readCheeps.ElementAt(0));
    }
}
