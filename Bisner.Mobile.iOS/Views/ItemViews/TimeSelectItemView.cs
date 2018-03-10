using System;
using Bisner.Mobile.Core.ViewModels.Booking;
using UIKit;

namespace Bisner.Mobile.iOS.Views.ItemViews
{
    public partial class TimeSelectItemView : UIView
    {
        public TimeSelectItemView (IntPtr handle) : base (handle)
        {
        }

		public string Time
		{
			get
			{
				return this.lblTime.Text;
			}

			set
			{
				this.lblTime.Text = value;
			}
		}

		private TimeBlockType _timeBlockType;
		public TimeBlockType TimeBlockType
		{
			get
			{
				return _timeBlockType;
			}

			set
			{
				_timeBlockType = value;
				if (_timeBlockType == TimeBlockType.FIFTEEN)
				{
					vw15.Hidden = false;
					vw30.Hidden = false;
					vw45.Hidden = false;
				}
				else if (_timeBlockType == TimeBlockType.THIRTY)
				{
					vw15.Hidden = true;
					vw30.Hidden = false;
					vw45.Hidden = true;
				}
				else
				{
					vw15.Hidden = true;
					vw30.Hidden = true;
					vw45.Hidden = true;
				}
			}
		}
    }
}