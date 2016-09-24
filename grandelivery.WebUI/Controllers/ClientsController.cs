using grandelivery.WebUI.Infrastructure.Repositories;
using grandelivery.WebUI.ViewModels;
using SX.WebCore;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace grandelivery.WebUI.Controllers
{
    [Authorize(Roles ="admin")]
    public sealed class ClientsController : BaseController
    {
        private static RepoClient _repo = new RepoClient();
        public static RepoClient Repo
        {
            get { return _repo; }
            set { _repo = value; }
        }


        private static readonly int _pageSize = 40;
        [HttpGet]
        public ActionResult List(int page=1)
        {
            var order = new SxOrder() { FieldName = "NikName", Direction = SortDirection.Asc };
            var filter = new SxFilter(page, _pageSize) { Order = order };
            var viewModel = Repo.Read(filter);

            if (page > 1 && !viewModel.Any())
                return new HttpNotFoundResult();

            ViewBag.Filter = filter;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> List(VMClient filterModel, SxOrder order, int page = 1)
        {
            var filter = new SxFilter(page, _pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };

            var viewModel = await _repo.ReadAsync(filter);

            if (page > 1 && !viewModel.Any())
                return new HttpNotFoundResult();

            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }
    }
}