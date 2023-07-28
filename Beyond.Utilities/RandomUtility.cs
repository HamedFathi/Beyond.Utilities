// ReSharper disable CheckNamespace
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable StringLiteralTypo

namespace Beyond.Utilities;

public static class RandomUtility
{
    public static string GenerateYouTubeLikeId()
    {
        var base64Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_";
        var random = new byte[8]; // YouTube uses 64 bits random values

        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(random);
        }

        var stringBuilder = new StringBuilder(11);
        for (var i = 0; i < 11; i++)
        {
            var index = i < 10 ? random[i % 8] : (byte)(random[0] ^ random[1] ^ random[2] ^ random[3] ^ random[4] ^ random[5] ^ random[6] ^ random[7]);
            index &= 0x3F; // Make sure index is within base64Chars' length
            stringBuilder.Append(base64Chars[index]);
        }
        return stringBuilder.ToString();
    }
    public static DateTime GetRandomDateTime(DateTime? startDateTime = null, DateTime? endDateTime = null)
    {
        var rnd = GetUniqueRandom();
        var rndYear = new Random().Next(-100, +100);
        var start = startDateTime ?? DateTime.Now.AddYears(rndYear);
        var end = endDateTime ?? DateTime.Now;
        var range = (end - start).Days;
        return start.AddDays(rnd.Next(range)).AddHours(rnd.Next(0, 24)).AddMinutes(rnd.Next(0, 60))
            .AddSeconds(rnd.Next(0, 60));
    }

    public static string GetRandomString(int length,
        string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890_")
    {
        if (length <= 0) throw new ArgumentException($"'{nameof(length)}' cannot be zero or negative.");

        using var crypto = RandomNumberGenerator.Create();

        var data = new byte[length];
        byte[]? buffer = null;

        var maxRandom = byte.MaxValue - (byte.MaxValue + 1) % chars.Length;

        crypto.GetBytes(data);

        var result = new char[length];

        for (var i = 0; i < length; i++)
        {
            var value = data[i];

            while (value > maxRandom)
            {
                buffer ??= new byte[1];
                crypto.GetBytes(buffer);
                value = buffer[0];
            }

            result[i] = chars[value % chars.Length];
        }

        return new string(result);
    }

    public static Random GetUniqueRandom()
    {
        return new Random(Guid.NewGuid().GetHashCode());
    }
}