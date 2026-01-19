namespace Sob_Need_Server.Utils.Helpers
{
    public class StringUtilities
    {
        // Generate slug 
        public static string GenerateSlug(string phrase)
        {
            if (string.IsNullOrEmpty(phrase)) return string.Empty;

            string str = phrase.ToLower();
            
            // Remove invalid characters
            str = System.Text.RegularExpressions.Regex.Replace(str, @"[^a-z0-9\s-]", "");

            // Convert multiple spaces into one space   
            str = System.Text.RegularExpressions.Regex.Replace(str, @"\s+", " ").Trim();

            // Cut and trim 
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            
            // Hyphens
            str = System.Text.RegularExpressions.Regex.Replace(str, @"\s", "-"); 

            return str;
        }
    }
}
