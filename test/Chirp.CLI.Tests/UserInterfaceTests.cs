namespace Chirp.CLI.Tests;

public class UserInterfaceTests
{
    [Fact]
    public void PrintCheepsToConsole()
    {
        var cheeps = new List<Program.Cheep>
        {
            new Program.Cheep("testUser", "testCheep", 1690981487), // unixtime =  02.08.2023 15.04.47
            new Program.Cheep("testUser2", "testCheep2", 1690981487)
        };

        var output = new StringWriter();
        Console.SetOut(output);

        UserInterface.PrintCheeps(cheeps);

        var expected = "testUser @ 02.08.2023 15.04.47: testCheep\n" +
                       "testUser2 @ 02.08.2023 15.04.47: testCheep2\n";

        Assert.Equal(expected, output.ToString());
    }
}
