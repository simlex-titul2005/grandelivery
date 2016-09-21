using SX.WebCore.MvcControllers.Abstract;
using System.Web.Mvc;

namespace grandelivery.WebUI.Controllers
{
    [Authorize]
    public abstract class BaseController : SxBaseController
    {
        [NonAction]
        protected string GetUserRole()
        {
            return User.IsInRole("admin") ? "admin" : User.IsInRole("customer") ? "customer" : User.IsInRole("carrier") ? "carrier" : null;
        }
    }
}