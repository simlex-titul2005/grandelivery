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

namespace grandelivery.WebUI.Infrastructure.Repositories
{
    public sealed class RepoOrder : SxDbRepository<int, Order, VMOrder>
    {
        public sealed override Order Create(Order model)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<Order>("dbo.add_order @df, @dt, @tdb, @tde, @uid, @cName, @cWeight, @cWidth, @cHeight, @cLength", new {
                    df=model.DestinationFrom,
                    dt=model.DestinationTo,
                    tdb=model.TakeDateBegin,
                    tde=model.TakeDateEnd,
                    uid=model.UserId,
                    cName=model.CargoName,
                    cWeight=model.CargoWeight,
                    cWidth=model.CargoWidth,
                    cHeight=model.CargoHeight,
                    cLength=model.CargoLength
                });

                return data.SingleOrDefault();
            }
        }

        public sealed override VMOrder[] Read(SxFilter filter)
        {
            var sb = new StringBuilder();
            sb.Append(SxQueryProvider.GetSelectString());
            sb.Append(" FROM D_ORDER AS do ");

            object param = null;
            var gws = getOrdersWhereString(filter, out param);
            sb.Append(gws);

            var defaultOrder = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            sb.Append(SxQueryProvider.GetOrderString(defaultOrder, filter.Order));

            sb.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", filter.PagerInfo.SkipCount, filter.PagerInfo.PageSize);

            //count
            var sbCount = new StringBuilder();
            sbCount.Append("SELECT COUNT(1) FROM D_ORDER AS do ");
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
            //query.Append(" WHERE (dal.[Description] LIKE '%'+@desc+'%' OR @desc IS NULL) ");

            //string desc = filter.WhereExpressionObject?.Description;

            //param = new
            //{
            //    desc = desc
            //};

            return query.ToString();
        }

        public sealed override Order Update(Order model)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<Order>("dbo.update_order @id, @df, @dt, @tdb, @tde, @cName, @cWeight, @cWidth, @cHeight, @cLength", new
                {
                    id=model.Id,
                    df = model.DestinationFrom,
                    dt = model.DestinationTo,
                    tdb = model.TakeDateBegin,
                    tde = model.TakeDateEnd,
                    cName = model.CargoName,
                    cWeight = model.CargoWeight,
                    cWidth = model.CargoWidth,
                    cHeight = model.CargoHeight,
                    cLength = model.CargoLength
                });

                return data.SingleOrDefault();
            }
        }
    }
}