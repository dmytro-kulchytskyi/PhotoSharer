using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PhotoSharer.MVC.ActionFilters
{
    public class LanguageActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.RouteData.Values.ContainsKey("language"))
            {
                var languageName = (string)filterContext.RouteData.Values["language"];
                var currentCultureInfo = CultureInfo.GetCultureInfo(languageName);
                if (currentCultureInfo != null)
                    Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture = currentCultureInfo;
            }
        }
    }
}
