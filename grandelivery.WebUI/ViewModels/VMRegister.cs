using SX.WebCore.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace grandelivery.WebUI.ViewModels
{
    [MetadataType(typeof(VMRegisterMetadata))]
    public sealed class VMRegister : SxVMRegister
    {
    }

    public sealed class VMRegisterMetadata
    {
        [Required(ErrorMessage ="Необходимо выбрать роль")]
        public string RoleName { get; set; }
    }
}