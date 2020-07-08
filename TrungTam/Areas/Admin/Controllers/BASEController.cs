using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TrungTam.Areas.Admin.Abstracts;
using TrungTam.Areas.Admin.Common;

namespace TrungTam.Areas.Admin.Controllers
{
    public class BASEController : Controller
    {
        // GET: Admin/BASE
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var session = (UserLogin)Session[CommonContants.USER_SESSION];
            if(session == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new {
                    controller = "Home", action = "Index"
                }));
            }
            base.OnActionExecuted(filterContext);
        }
    }
}