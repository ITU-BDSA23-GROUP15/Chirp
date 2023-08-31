if (args.Length < 3)
{
    Console.WriteLine("");
    return;
}

if (args[0] == "say")
{
    var random = new Random();
    var message = args[1];
    var frequency = int.Parse(args[2]);

    foreach (var i in Enumerable.Range(1, frequency))
    {
        var wait = random.Next(100, 1000);
        await Task.Delay(TimeSpan.FromMilliseconds(wait));
        Console.WriteLine($"{message} {wait}");
    }
}