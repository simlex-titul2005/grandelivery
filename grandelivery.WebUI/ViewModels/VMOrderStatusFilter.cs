using grandelivery.WebUI.Models;

namespace grandelivery.WebUI.ViewModels
{
    public sealed class VMOrderStatusFilter
    {
        public int OrderStatus { get; set; }

        public string OrderStatusName
        {
            get
            {
                switch (OrderStatus)
                {
                    case (int)Order.OrderStatus.Viewed:
                        return "Просмотрено";
                    case (int)Order.OrderStatus.DoneAtWork:
                        return "Принято в работу";
                    case (int)Order.OrderStatus.OnTheWay:
                        return "В пути";
                    case (int)Order.OrderStatus.Delivered:
                        return "Доставлено";
                    case (int)Order.OrderStatus.Canceled:
                        return "Отменено";
                    default:
                        return "Не определено";
                }
            }
        }

        public int Count { get; set; }
    }
}