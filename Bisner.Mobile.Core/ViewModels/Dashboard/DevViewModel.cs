using System;
using System.Threading.Tasks;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.Base;

namespace Bisner.Mobile.Core.ViewModels.Dashboard
{
    public class DevViewModel : BaseViewModel
    {
        #region Constructor & Init

        private string _serverIp;

        public DevViewModel(IPlatformService platformService) : base(platformService)
        {
        }

        public async Task Init()
        {
            try
            {
                    
            }
            catch (Exception ex)
            {
                ExceptionService.HandleException(ex);
            }
        }

        #endregion Constructor & init

        #region Server info

        public string ServerIp
        {
            get => _serverIp;
            set { _serverIp = value; RaisePropertyChanged(() => ServerIp); }
        }

        #endregion Server info
    }
}