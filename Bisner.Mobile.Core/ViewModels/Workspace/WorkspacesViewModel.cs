using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.Base;

namespace Bisner.Mobile.Core.ViewModels.Workspace
{
    /// <summary>
    /// ViewModel to display list of Workspaces
    /// </summary>
    public class WorkspacesViewModel : BaseViewModel
    {
        #region Constructor

        public WorkspacesViewModel(IPlatformService platformService) : base(platformService)
        {
        }

        #endregion Constructor
    }
}
