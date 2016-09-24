using SX.WebCore.ViewModels;

namespace grandelivery.WebUI.ViewModels
{
    public sealed class VMClient : SxVMAppUser
    {
        public string RoleName { get; set; }

        public int OrderCount { get; set; }
    }
}