namespace Chirp.SimpleDB.Tests;

public class CSVDatabaseTests
{
    [Fact]
    public void StoreAndReadCheepsTest() {
        var cheeps = new List<Cheep>
        {
            new Cheep("testUser", "testCheep", 1690981487),
            new Cheep("testUser2", "testCheep2", 1690981487)
        };

        var database = CSVDatabase<Cheep>.Instance;
        database.Store(cheeps);

        var readCheeps = database.Read(10);

        Assert.Equal(cheeps, readCheeps);
    }
}
