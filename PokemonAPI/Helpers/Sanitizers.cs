using System.Text.RegularExpressions;

namespace PokemonAPI.Helpers;

public static class Sanitizers
{
    public static string SanitizeStringFromSpecialCharacters(this string originalString)
    {
        return Regex.Replace(originalString, "[^a-zA-Z0-9 ]+", " ", RegexOptions.Compiled);
    }
}