using System;
using System.ComponentModel.DataAnnotations;
using static grandelivery.WebUI.Models.Order;

namespace grandelivery.WebUI.ViewModels
{
    public sealed class VMOrder
    {
        public int Id { get; set; }

        public DateTime DateCreate { get; set; }

        public DateTime DateUpdate { get; set; }

        [Required(ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(150, ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Display(Name ="Адрес отправки")]
        public string DestinationFrom { get; set; }

        [Required(ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(150, ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Display(Name = "Адрес назначения")]
        public string DestinationTo { get; set; }

        [Required(ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Начальная дата"), UIHint("_EditDate")]
        public DateTime TakeDateBegin { get; set; }

        [Required(ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Конечная дата"), UIHint("_EditDate")]
        public DateTime TakeDateEnd { get; set; }

        [Required(ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(150, ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Display(Name = "Наименование груза")]
        public string CargoName { get; set; }

        [Required(ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Вес груза, кг")]
        [Range(typeof(decimal), "0,00", "1000000", ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "ValidDecimalFild")]
        public decimal? CargoWeight { get; set; }

        [Required(ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [Range(typeof(decimal), "0,00", "1000000", ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "ValidDecimalFild")]
        [Display(Name = "Ширина груза, м")]
        public decimal? CargoWidth { get; set; }

        [Required(ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [Range(typeof(decimal), "0,00", "1000000", ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "ValidDecimalFild")]
        [Display(Name = "Высота груза, м")]
        public decimal? CargoHeight { get; set; }

        [Required(ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [Range(typeof(decimal), "0,00", "1000000", ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "ValidDecimalFild")]
        [Display(Name = "Длина груза, м")]
        public decimal? CargoLength { get; set; }
        
        [Display(Name = "Статус заявки")]
        public OrderStatus? Status { get; set; }

        [MaxLength(400, ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Display(Name = "Комментарий"), DataType(DataType.MultilineText)]
        public string Comment { get; set; }

        [MaxLength(400, ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Display(Name = "Комментарий администратора"), DataType(DataType.MultilineText)]
        public string AdminComment { get; set; }

        public string UserId { get; set; }
    }
}