using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PhotoSharer.MVC.Helpers
{
    public class IdentityHelper
    {
        public static Guid UserId
        {
            get
            {
                if (HttpContext.Current == null)
                    throw new HttpException("No HttpContext");

                if (!HttpContext.Current.User.Identity.IsAuthenticated)
                    return Guid.Empty;

                return Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            }
        }

        public static bool IsAuthenticated
        {
            get
            {
                if (HttpContext.Current == null)
                    throw new HttpException("No HttpContext");

                return HttpContext.Current.User.Identity.IsAuthenticated;
            }
        }
    }
}
