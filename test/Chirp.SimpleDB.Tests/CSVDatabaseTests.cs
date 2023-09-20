namespace Chirp.SimpleDB.Tests;

using SimpleDB;

public class CSVDatabaseTests
{
    [Fact]
    public void StoreAndReadCheepsTest() {
        // Arrange
        var cheep = new Program.Cheep("testUser", "testCheep", 1690981487);
        var database = CSVDatabase.CSVDatabase<Program.Cheep>.Instance;

        // Act
        database.Store(cheep);

        var readCheeps = database.Read(10);

        // Assert
        Assert.Equal(cheep, readCheeps.ElementAt(0));
    }
}
