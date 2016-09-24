using Dapper;
using grandelivery.WebUI.ViewModels;
using SX.WebCore;
using SX.WebCore.Providers;
using SX.WebCore.Repositories.Abstract;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace grandelivery.WebUI.Infrastructure.Repositories
{
    public sealed class RepoClient : SxDbRepository<string, SxAppUser, VMClient>
    {
        public sealed override VMClient[] Read(SxFilter filter)
        {
            var sb = new StringBuilder();
            sb.Append(SxQueryProvider.GetSelectString(new string[] {
                "anu.*",
                "(SELECT TOP(1) anr2.Name FROM AspNetRoles AS anr2 WHERE anr2.Id=anur.RoleId) AS RoleName",
                "(SELECT COUNT(1) FROM D_ORDER AS do WHERE do.UserId=anu.Id) AS OrderCount"
            }));
            sb.Append(" FROM AspNetUsers AS anu ");
            sb.Append(" JOIN AspNetUserRoles AS anur ON anur.UserId = anu.Id ");
            sb.Append(" JOIN AspNetRoles AS anr ON anr.Id = anur.RoleId AND anr.Name IN('customer', 'carrier', 'admin') ");

            object param = null;
            var gws = getClientsWhereString(filter, out param);
            sb.Append(gws);

            var defaultOrder = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            sb.Append(SxQueryProvider.GetOrderString(defaultOrder, filter.Order));

            sb.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", filter.PagerInfo.SkipCount, filter.PagerInfo.PageSize);

            //count
            var sbCount = new StringBuilder();
            sbCount.Append("SELECT COUNT(1) FROM AspNetUsers AS anu ");
            sbCount.Append(" JOIN AspNetUserRoles AS anur ON anur.UserId = anu.Id ");
            sbCount.Append(" JOIN AspNetRoles AS anr ON anr.Id = anur.RoleId AND anr.Name IN('customer', 'carrier', 'admin') ");
            sbCount.Append(gws);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<VMClient>(sb.ToString(), param: param);
                filter.PagerInfo.TotalItems = conn.Query<int>(sbCount.ToString(), param: param).SingleOrDefault();
                return data.ToArray();
            }
        }
        private static string getClientsWhereString(SxFilter filter, out object param)
        {
            param = null;
            var query = new StringBuilder();

            //string desc = filter.WhereExpressionObject?.Description;

            //param = new
            //{
            //    desc = desc
            //};

            return query.ToString();
        }
    }
}