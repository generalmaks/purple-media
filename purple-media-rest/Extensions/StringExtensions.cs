namespace purple_media_rest.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Returns all starting indices of a specified substring within the given text, performing a case-insensitive search.
    /// </summary>
    /// <param name="text">The source string to search in.</param>
    /// <param name="snippet">The substring to look for.</param>
    /// <returns>An array of integers representing the starting positions of all occurrences of the substring in text</returns>
    public static int[] AllIndices(this string text, string snippet)
    {
        var indices = new List<int>();
        int index = 0;
        while ((index = text.IndexOf(snippet, index, StringComparison.OrdinalIgnoreCase)) != -1)
        {
            indices.Add(index);
            index += snippet.Length;
        }
        return indices.ToArray();
    }
    
    /// <summary>
    /// Returns all starting indices of multiple substrings within the given text, performing a case-insensitive search.
    /// </summary>
    /// <param name="text">The source string to search in.</param>
    /// <param name="snippets">An array of substrings to look for.</param>
    /// <returns>An array of integers representing the starting positions of all occurrences of each substring in text</returns>
    public static List<int> AllIndices(this string text, string[] snippets)
    {
        var indices = new List<int>();
        foreach (var snippet in snippets)
        {
            indices.AddRange(text.AllIndices(snippet));
        }
        return indices;
    }
}