// ReSharper disable UnusedMember.Global
namespace Beyond.Utilities;

public static class CharUtility
{
    public static IEnumerable<char> GetAllLetters()
    {
        var chars = GetAllPrintableChars()
            .Where(char.IsLetter)
            .ToArray();
        return chars;
    }

    public static IEnumerable<char> GetAllDigits()
    {
        var chars = GetAllPrintableChars()
            .Where(char.IsDigit)
            .ToArray();
        return chars;
    }

    public static IEnumerable<char> GetAllLettersAndDigits()
    {
        var chars = GetAllUnicodeChars()
            .Where(char.IsLetterOrDigit)
            .ToArray();
        return chars;
    }

    public static IEnumerable<char> GetAllPrintableChars()
    {
        var chars = GetAllUnicodeChars()
            .Where(c => !char.IsControl(c) && !char.IsWhiteSpace(c))
            .ToArray();
        return chars;
    }

    public static IEnumerable<char> GetAllUnicodeChars()
    {
        var chars = Enumerable.Range(0, char.MaxValue + 1)
            .Select(i => (char)i)
            .ToArray();
        return chars;
    }
}