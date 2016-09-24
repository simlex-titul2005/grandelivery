using SX.WebCore;
using SX.WebCore.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace grandelivery.WebUI.Models
{
    [Table("D_ORDER_TRACK")]
    public class OrderTrack : SxDbUpdatedModel<int>
    {
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        [Required, MaxLength(128)]
        public string UserId { get; set; }
        public virtual SxAppUser User { get; set; }

        public bool IsActive { get; set; }
    }
}