using grandelivery.WebUI.Infrastructure.Repositories;
using grandelivery.WebUI.Models;
using grandelivery.WebUI.ViewModels;
using SX.WebCore;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace grandelivery.WebUI.Controllers
{
    [Authorize(Roles ="admin,customer")]
    public sealed class OrdersController : BaseController
    {
        private static RepoOrder _repo = new RepoOrder();
        public static RepoOrder Repo
        {
            get { return _repo; }
            set { _repo = value; }
        }

        private static readonly int _pageSize = 20;
        [HttpGet]
        public ActionResult List(int page = 1)
        {
            var role = GetUserRole();
            var filter = new SxFilter(page, _pageSize) { AddintionalInfo = new object[] { role } };
            var viewModel = Repo.Read(filter);

            if (page > 1 && !viewModel.Any())
                return new HttpNotFoundResult();

            ViewBag.Filter = filter;

            return PartialView("_List", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> List(VMOrder filterModel, SxOrder order, int page = 1)
        {
            var filter = new SxFilter(page, _pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };

            var viewModel = await _repo.ReadAsync(filter);

            if (page > 1 && !viewModel.Any())
                return new HttpNotFoundResult();

            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public ActionResult Edit(int? orderId)
        {
            var userRole = GetUserRole();
            if (!Equals(userRole, "customer")) return new HttpNotFoundResult();

            var data = orderId.HasValue ? Repo.GetByKey(orderId) : new Order();
            if (orderId.HasValue && data == null) return new HttpNotFoundResult();

            var viewModel = Mapper.Map<Order, VMOrder>(data);

            return View(viewModel);
        }
    }
}