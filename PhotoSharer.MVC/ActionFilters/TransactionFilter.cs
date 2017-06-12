using Ninject;
using PhotoSharer.Business;
using PhotoSharer.MVC.NInjectIOC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PhotoSharer.MVC.ActionFilters
{
    public class TransactionFilter : IActionFilter
    {
        private readonly IUnitOfWork unitOfWork;

        public TransactionFilter()
        {
            unitOfWork = IOC.Resolve<IUnitOfWork>();
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            unitOfWork.BeginTransaction();
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            unitOfWork.Commit();
        }
    }
}
