using grandelivery.WebUI.Infrastructure.Repositories;
using grandelivery.WebUI.Models;
using grandelivery.WebUI.ViewModels;
using Microsoft.AspNet.Identity;
using SX.WebCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace grandelivery.WebUI.Controllers
{
    [Authorize(Roles = "admin,customer,carrier")]
    public sealed class OrdersController : BaseController
    {
        private static RepoOrder _repo = new RepoOrder();
        public static RepoOrder Repo
        {
            get { return _repo; }
        }

        private static readonly int _pageSize = 40;
        [HttpGet]
        public ActionResult List(int page = 1)
        {
            var order = new SxOrder() { FieldName = "DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageSize) { Order = order, AddintionalInfo = new object[] { getQueryUserId(), GetUserRole(), null } };
            var viewModel = Repo.Read(filter);

            if (page > 1 && !viewModel.Any())
                return new HttpNotFoundResult();

            ViewBag.Filter = filter;

            return PartialView("_List", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> List(VMOrder filterModel, SxOrder order, int page = 1)
        {
            var status = Request.Form.Get("status");

            var filter = new SxFilter(page, _pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel, AddintionalInfo = new object[] { getQueryUserId(), GetUserRole(), status != null ? Convert.ToInt32(status) : (int?)null } };

            var viewModel = await Repo.ReadAsync(filter);

            if (page > 1 && !viewModel.Any())
                return new HttpNotFoundResult();

            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }

        private string getQueryUserId()
        {
            string userId = User.Identity.GetUserId();
            var role = GetUserRole();
            if (Equals(role, "admin") || Equals(role, "carrier"))
                userId = null;

            return userId;
        }

        public async Task<ActionResult> Edit(int? orderId)
        {
            var userRole = GetUserRole();

            var date = DateTime.Now;
            var data = orderId.HasValue ? Repo.GetByKey(orderId) : new Order()
            {
                TakeDateBegin = date.AddHours(1),
                TakeDateEnd = date.AddDays(1),
            };

            if (orderId.HasValue && data == null) return new HttpNotFoundResult();

            if (Equals(userRole, "admin") && orderId.HasValue && data != null && !data.Status.HasValue)
                data.Status = await Repo.ChangeStatus(data.Id, Order.OrderStatus.Viewed);

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
            if (ModelState.IsValid)
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

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(List<int> ids)
        {
            await Repo.DeleteAsync(ids);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<JsonResult> TakeCargo(int orderId)
        {
            var userId = User.Identity.GetUserId();
            var data = await Repo.TakeCargoAsync(orderId, userId);
            return Json(data);
        }

        [HttpPost]
        public async Task<JsonResult> UntakeCargo(int orderId)
        {
            var data = await Repo.UntakeCargoAsync(orderId);
            return Json(data);
        }
    }
}