using grandelivery.WebUI.Infrastructure.Repositories;
using grandelivery.WebUI.Models;
using grandelivery.WebUI.ViewModels;
using Microsoft.AspNet.Identity;
using SX.WebCore;
using System;
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
            var order = new SxOrder() { FieldName = "DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageSize) { Order=order, AddintionalInfo = new object[] { role } };
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

            var date = DateTime.Now;
            var data = orderId.HasValue ? Repo.GetByKey(orderId) : new Order() {
                TakeDateBegin = date.AddHours(1),
                TakeDateEnd=date.AddDays(1),
            };

            if (orderId.HasValue && data == null) return new HttpNotFoundResult();

            var viewModel = Mapper.Map<Order, VMOrder>(data);
            if (!orderId.HasValue)
            {
                viewModel.CargoHeight = null;
                viewModel.CargoLength = null;
                viewModel.CargoWeight = null;
                viewModel.CargoWidth = null;
            }

            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(VMOrder model)
        {
            if(ModelState.IsValid)
            {
                var isNew = model.Id == 0;
                var redactModel = Mapper.Map<VMOrder, Order>(model);
                redactModel.UserId = User.Identity.GetUserId();

                if (isNew)
                    Repo.Create(redactModel);
                else
                    Repo.Update(redactModel);

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }
    }
}