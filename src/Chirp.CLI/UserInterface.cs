﻿namespace Chirp;

public static class UserInterface
{
public static void PrintCheeps(IEnumerable<Cheep> cheeps)
{
    foreach (var cheep in cheeps)
    {
        var formattedDateTime = UnixTimeStampToDateTime(cheep.Timestamp);
        Console.WriteLine($"{cheep.Author} @ {formattedDateTime}: {cheep.Message}");
    }
}

    //code snatched from: https://stackoverflow.com/questions/249760/how-can-i-convert-a-unix-timestamp-to-datetime-and-vice-versa
    public static DateTime UnixTimeStampToDateTime(long unixTimeStamp )
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds( unixTimeStamp ).ToLocalTime();
        return dateTime;
    }
}