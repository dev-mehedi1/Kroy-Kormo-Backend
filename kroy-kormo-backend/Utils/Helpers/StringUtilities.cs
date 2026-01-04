namespace Sob_Need_Server.Utils.Helpers
{
    public class StringUtilities
    {
        // Generate slug 
        public static string GenerateSlug(string slug)
        {
            if (string.IsNullOrEmpty(slug)) return string.Empty;
            return slug
                .ToLower()
                .Replace(" ", "-")
                .Replace("'", "")
                .Replace("\"", "")
                .Replace(".", "")
                .Replace(",", "")
                .Trim();
        }
    }
}
