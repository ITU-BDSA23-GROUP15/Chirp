namespace Chirp.SimpleDB.Tests;

using SimpleDB;

public class CSVDatabaseTests
{
    [Fact]
    public void StoreAndReadCheepsTest() {
        // Arrange
        var cheeps = new List<Program.Cheep>
        {
            new Program.Cheep("testUser", "testCheep", 1690981487),
            new Program.Cheep("testUser2", "testCheep2", 1690981487)
        };
        // Act
        var database = CSVDatabase.CSVDatabase<Program.Cheep>.Instance;
        database.Store(cheeps);

        var readCheeps = database.Read(10);
        // Assert
        Assert.Equal(cheeps, readCheeps);
    }
}
