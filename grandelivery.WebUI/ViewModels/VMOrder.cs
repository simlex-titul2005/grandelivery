using System;
using System.ComponentModel.DataAnnotations;

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
        [Display(Name = "Начальная дата")]
        public DateTime TakeDateBegin { get; set; }

        [Required(ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Конечная дата")]
        public DateTime TakeDateEnd { get; set; }

        [Required(ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(150, ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Display(Name = "Наименование груза")]
        public string CargoName { get; set; }

        [Display(Name = "Вес груза, кг")]
        public decimal CargoWeight { get; set; }

        [Display(Name = "Ширина груза, м")]
        public decimal CargoWidth { get; set; }

        [Display(Name = "Высота груза, м")]
        public decimal CargoHeight { get; set; }

        [Display(Name = "Длина груза, м")]
        public decimal CargoLength { get; set; }
    }
}