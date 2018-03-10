using Bisner.Mobile.Core.ViewModels.Dashboard.Models.Items;

namespace Bisner.Mobile.Core.ViewModels.Dashboard.Models.Cards
{
    public class DashboardWelcomeCard : DashboardCardSectionBase<DashboardItemBase>
    {
        #region Fields and consctructor

        public DashboardWelcomeCard()
        {

        }

        #endregion Fields and consctructor

        #region Properties

        public override string Title { get { return "Welcome"; } }

        public override string FooterText { get { return "Learn more"; } }

        #endregion Properties
    }
}
