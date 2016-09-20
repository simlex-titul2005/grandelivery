using System.Web.Mvc;
using SX.WebCore.MvcControllers;
using grandelivery.WebUI.ViewModels;
using SX.WebCore.ViewModels;
using System.Threading.Tasks;

namespace grandelivery.WebUI.Controllers
{
    public sealed class AccountController : SxAccountController
    {
        [HttpGet, AllowAnonymous]
        public sealed override ActionResult Register()
        {
            var viewModel = new VMRegister();
            return View(viewModel);
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public sealed override async Task<ActionResult> Register(SxVMRegister model)
        {
            var result = await base.Register(model);
            if(!ModelState.IsValid)
            {
                var viewModel = Mapper.Map<SxVMRegister, VMRegister>(model);
                return View(viewModel);
            }

            else return result;
        }
    }
}