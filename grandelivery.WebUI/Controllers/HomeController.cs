using System.Web.Mvc;

namespace grandelivery.WebUI.Controllers
{
    [Authorize(Roles = "admin,customer,carrier")]
    public sealed class HomeController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}