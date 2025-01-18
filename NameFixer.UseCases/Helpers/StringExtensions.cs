using System.Text;

namespace NameFixer.UseCases.Helpers;

public static class StringExtensions
{
    private static readonly Dictionary<char, char> PolishToLatinMap = new()
    {
        {
            'Ą', 'A'
        },
        {
            'Ć', 'C'
        },
        {
            'Ę', 'E'
        },
        {
            'Ł', 'L'
        },
        {
            'Ń', 'N'
        },
        {
            'Ó', 'O'
        },
        {
            'Ś', 'S'
        },
        {
            'Ź', 'Z'
        },
        {
            'Ż', 'Z'
        }
    };

    public static string Capitalize(this string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return value;

        return char.ToUpper(value[0]) + value[1..]
            .ToLower();
    }

    public static string RemoveAccents(this string text)
    {
        if (string.IsNullOrEmpty(text)) return text;

        var result = new StringBuilder();

        foreach (var c in text)
            result.Append(PolishToLatinMap.GetValueOrDefault(c, c));

        return result.ToString();
    }
}