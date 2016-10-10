using grandelivery.WebUI.Models;
using grandelivery.WebUI.ViewModels;
using SX.WebCore;
using SX.WebCore.Repositories.Abstract;
using System.Data.SqlClient;
using Dapper;
using System.Text;
using SX.WebCore.Providers;
using static SX.WebCore.HtmlHelpers.SxExtantions;
using System.Linq;
using static grandelivery.WebUI.Models.Order;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace grandelivery.WebUI.Infrastructure.Repositories
{
    public sealed class RepoOrder : SxDbRepository<int, Order, VMOrder>
    {
        public sealed override Order Create(Order model)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<Order>("dbo.add_order @df, @dt, @tdb, @tde, @uid, @cName, @cWeight, @cWidth, @cHeight, @cLength, @comment, @adminComment", new
                {
                    df = model.DestinationFrom,
                    dt = model.DestinationTo,
                    tdb = model.TakeDateBegin,
                    tde = model.TakeDateEnd,
                    uid = model.UserId,
                    cName = model.CargoName,
                    cWeight = model.CargoWeight,
                    cWidth = model.CargoWidth,
                    cHeight = model.CargoHeight,
                    cLength = model.CargoLength,
                    comment=model.Comment,
                    adminComment=model.AdminComment
                });

                return data.SingleOrDefault();
            }
        }

        public sealed override VMOrder[] Read(SxFilter filter)
        {
            var sb = new StringBuilder();
            sb.Append(SxQueryProvider.GetSelectString(new string[] {
                "do.*",
                "(ROUND(do.CargoLength * do.CargoWidth * do.CargoHeight, 2)) AS Volume"
            }));
            sb.Append(" FROM D_ORDER AS do ");
            sb.Append(" JOIN AspNetUsers AS anu ON anu.Id = do.UserId ");
            sb.Append(" JOIN AspNetUserRoles AS anur ON anur.UserId = anu.Id ");
            sb.Append(" JOIN AspNetRoles AS anr ON anr.Id = anur.RoleId AND anr.Name IN ('customer', 'carrier', 'admin') ");

            object param = null;
            var gws = getOrdersWhereString(filter, out param);
            sb.Append(gws);

            var defaultOrder = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            sb.Append(SxQueryProvider.GetOrderString(defaultOrder, filter.Order, new Dictionary<string, string> {
                ["DateCreate"]="do.DateCreate"
            }));

            sb.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", filter.PagerInfo.SkipCount, filter.PagerInfo.PageSize);

            //count
            var sbCount = new StringBuilder();
            sbCount.Append("SELECT COUNT(1) FROM D_ORDER AS do ");
            sbCount.Append("JOIN AspNetUsers AS anu ON anu.Id = do.UserId ");
            sbCount.Append("JOIN AspNetUserRoles AS anur ON anur.UserId = anu.Id ");
            sbCount.Append("JOIN AspNetRoles AS anr ON anr.Id = anur.RoleId AND anr.Name IN ('customer', 'carrier', 'admin') ");
            sbCount.Append(gws);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<VMOrder>(sb.ToString(), param: param);
                filter.PagerInfo.TotalItems = conn.Query<int>(sbCount.ToString(), param: param).SingleOrDefault();
                return data.ToArray();
            }
        }
        private static string getOrdersWhereString(SxFilter filter, out object param)
        {
            param = null;
            var query = new StringBuilder();

            var userId = filter.AddintionalInfo != null && filter.AddintionalInfo[0] != null ? (string)filter.AddintionalInfo[0] : null;
            var roleName = filter.AddintionalInfo != null && filter.AddintionalInfo[1] != null ? (string)filter.AddintionalInfo[1] : null;
            var status = filter.AddintionalInfo != null && filter.AddintionalInfo[2] != null ? (int)filter.AddintionalInfo[2] : (int?)null;

            query.Append(" WHERE (do.UserId=@userId OR @userId IS NULL) ");
            if(roleName!=null && Equals(roleName, "carrier"))
                query.Append(" AND (do.Status IN (1, 2) ) ");
            if(status!=null)
                query.Append(" AND ((@status <> -1 AND do.[Status]=@status) OR (@status=-1 AND do.[Status] IS NULL)) ");

            param = new
            {
                userId = userId,
                status= status
            };

            return query.ToString();
        }

        public sealed override Order Update(Order model)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<Order>("dbo.update_order @id, @df, @dt, @tdb, @tde, @cName, @cWeight, @cWidth, @cHeight, @cLength, @comment, @adminComment, @status", new
                {
                    id = model.Id,
                    df = model.DestinationFrom,
                    dt = model.DestinationTo,
                    tdb = model.TakeDateBegin,
                    tde = model.TakeDateEnd,
                    cName = model.CargoName,
                    cWeight = model.CargoWeight,
                    cWidth = model.CargoWidth,
                    cHeight = model.CargoHeight,
                    cLength = model.CargoLength,
                    comment = model.Comment,
                    adminComment = model.AdminComment,
                    status=model.Status
                });

                return data.SingleOrDefault();
            }
        }

        public async Task<OrderStatus> ChangeStatus(int orderId, OrderStatus status)
        {
            return await Task.Run(() =>
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    var data = connection.Execute("dbo.change_order_status @orderId, @status", new { orderId = orderId, status = status });
                    return status;
                }
            });
        }

        public async Task<int> DeleteAsync(List<int> ids)
        {
            return await Task.Run(() =>
            {

                if (ids == null || !ids.Any()) return 0;

                var sb = new StringBuilder();
                ids.ForEach(x => { sb.AppendFormat(", {0}", x); });
                sb.Remove(0, 2);

                var query = "DELETE FROM D_ORDER WHERE Id IN (" + sb.ToString() + ")";

                using (var connection = new SqlConnection(ConnectionString))
                {
                    var data = connection.Query<int>(query);
                    return data.SingleOrDefault();
                }
            });
        }

        public async Task<int> TakeCargoAsync(int orderId, string userId)
        {
            return await Task.Run(()=> {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    var data = connection.Query<int>("dbo.take_cargo @orderId, @userId", new { orderId=orderId, userId=userId});
                    return orderId;
                }
            });
        }

        public async Task<int> UntakeCargoAsync(int orderId)
        {
            return await Task.Run(() => {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    var data = connection.Query<int>("dbo.untake_cargo @orderId", new { orderId = orderId });
                    return orderId;
                }
            });
        }

        public VMOrderStatusFilter[] GetOrdersStatusFilters()
        {
            var query = new StringBuilder();
            query.Append("SELECT do.[Status], COUNT(1) AS[Count] ");
            query.Append("FROM D_ORDER AS do ");
            query.Append("JOIN AspNetUsers AS anu ON anu.Id = do.UserId ");
            query.Append("JOIN AspNetUserRoles AS anur ON anur.UserId = anu.Id ");
            query.Append("JOIN AspNetRoles AS anr ON anr.Id = anur.RoleId AND anr.Name IN ('customer', 'carrier', 'admin') ");
            query.Append("GROUP BY do.[Status] ");
            query.Append("ORDER BY do.[Status] ");

            var data = new dynamic[0];
            var viewData = new VMOrderStatusFilter[0];
            using (var connection = new SqlConnection(ConnectionString))
            {
                data = connection.Query<dynamic>(query.ToString()).ToArray();
            }

            if (!data.Any()) return viewData;

            viewData = new VMOrderStatusFilter[data.Length];
            dynamic item = null;
            for (int i = 0; i < data.Length; i++)
            {
                item = data[i];
                viewData[i] = new VMOrderStatusFilter { Count = item.Count, OrderStatus = item.Status==null ? -1 : item.Status };
            }

            return viewData;
        }
    }
}