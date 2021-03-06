﻿using SX.WebCore;
using SX.WebCore.Abstract;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace grandelivery.WebUI.Models
{
    [Table("D_ORDER")]
    public class Order : SxDbUpdatedModel<int>
    {
        /// <summary>
        /// Адрес, откуда доставлять
        /// </summary>
        [Required, MaxLength(150)]
        public string DestinationFrom { get; set; }

        /// <summary>
        /// Адрес, куда доставлять
        /// </summary>
        [Required, MaxLength(150)]
        public string DestinationTo { get; set; }

        /// <summary>
        /// Дата забора, с
        /// </summary>
        public DateTime TakeDateBegin { get; set; }

        /// <summary>
        /// Дата забора, по
        /// </summary>
        public DateTime TakeDateEnd { get; set; }

        [MaxLength(128), Required]
        public string UserId { get; set; }
        public virtual SxAppUser User { get; set; }

        /// <summary>
        /// Наименование груза
        /// </summary>
        [MaxLength(150), Required]
        public string CargoName { get; set; }

        /// <summary>
        /// Вес груза
        /// </summary>
        public decimal CargoWeight { get; set; }

        /// <summary>
        /// Ширина груза
        /// </summary>
        public decimal CargoWidth { get; set; }

        /// <summary>
        /// Высота груза
        /// </summary>
        public decimal CargoHeight { get; set; }

        /// <summary>
        /// Длина груза
        /// </summary>
        public decimal CargoLength { get; set; }

        public OrderStatus? Status { get; set; }
        public enum OrderStatus : byte
        {
            /// <summary>
            /// Просмотрено
            /// </summary>
            [Description("Просмотрено")]
            Viewed = 0,

            /// <summary>
            /// Принято в работу
            /// </summary>
            [Description("Принято в работу")]
            DoneAtWork = 1,

            /// <summary>
            /// В пути
            /// </summary>
            [Description("В пути")]
            OnTheWay = 2,

            /// <summary>
            /// Доставлено
            /// </summary>
            [Description("Доставлено")]
            Delivered = 3,

            /// <summary>
            /// Отменено
            /// </summary>
            [Description("Отменено")]
            Canceled = 4
        }

        [MaxLength(400)]
        public string Comment { get; set; }

        [MaxLength(400)]
        public string AdminComment { get; set; }
    }
}