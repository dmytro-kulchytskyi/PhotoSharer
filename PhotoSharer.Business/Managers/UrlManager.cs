using System.Text.RegularExpressions;

namespace PhotoSharer.Business.Managers
{
    public class UrlManager
    {
        public static string GetSafeString(string line)
        {
            var result = Regex.Replace(line, @"\W+", "-").Trim('-');

            if (string.IsNullOrWhiteSpace(result))
                result = "_";

            return result;
        }
    }
}