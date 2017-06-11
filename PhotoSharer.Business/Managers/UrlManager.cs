using System.Text.RegularExpressions;

namespace PhotoSharer.Business.Managers
{
    public class UrlManager
    {
        public static string GetSafeString(string line)
        {
            var result = Regex.Replace(line, @"\W+", "_").Trim('_');

            if (string.IsNullOrWhiteSpace(result))
                result = "_";

            return result;
        }
    }
}