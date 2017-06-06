using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PhotoSharer.Business.Managers
{
    public class UrlManager
    {
        public static string GetSafeString(string line)
        {
            var result = Regex.Replace(line, @"\W+", "-").Trim('-');

            return result;
        }
    }
}
