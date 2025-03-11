using System.Text.RegularExpressions;

namespace PokemonAPIBusinessLayer.Helpers;

/// <summary>
/// Class for sanitizing methods
/// </summary>
public static class Sanitizers
{
    /// <summary>
    /// Sanitizes a string from special characters
    /// </summary>
    /// <param name="originalString">original string to sanitize</param>
    /// <returns>Sanitized string</returns>
    public static string SanitizeStringFromSpecialCharacters(this string originalString)
    {
        return Regex.Replace(originalString, "[^a-zA-Z0-9 ]+", " ", RegexOptions.Compiled);
    }
}