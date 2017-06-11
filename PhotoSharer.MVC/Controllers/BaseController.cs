using Ninject;
using PhotoSharer.Business;
using PhotoSharer.MVC.NInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PhotoSharer.MVC.Controllers
{
    public class BaseController : Controller
    {
        [Inject]
        public IUnitOfWork UnitOfWork;

        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if (!filterContext.IsChildAction)
            {
                var unitOfWork = NInject.IOC.Resolve<IUnitOfWork>();
                unitOfWork.Commit();
            }
        }
    }
}
