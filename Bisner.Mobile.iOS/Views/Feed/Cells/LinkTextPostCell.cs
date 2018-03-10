using System;
using System.Collections.Generic;
using Bisner.Mobile.Core.ViewModels.Feed.Items;
using Cirrious.FluentLayouts.Touch;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Feed.Cells
{
	partial class LinkTextPostCell : TextPostCellBase<FeedLinkTextPost>
    {
        #region Constructor

        public LinkTextPostCell (IntPtr handle) : base (handle)
		{
        }

        #endregion Constructor

        #region Overrides

	    protected override List<UIView> AddChildControls()
	    {
	        return new List<UIView>
	        {
	            
	        };
	    }

	    protected override List<FluentLayout> AddContentConstraintsBelowText(UIView contentContainer)
	    {
	        return new List<FluentLayout>
	        {
	            
	        };
	    }

	    #endregion Overrides
    }
}
