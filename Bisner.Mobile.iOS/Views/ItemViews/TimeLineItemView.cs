using System;
using Bisner.Mobile.Core.ViewModels.Booking;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace Bisner.Mobile.iOS.Views.ItemViews
{
    public partial class TimeLineItemView : MvxCollectionViewCell
    {
        public TimeLineItemView(IntPtr handle) : base(handle)
        {
        }

        public string TimeString
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

        TimeBlockType _timeBlockType;
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

        public void InitStyle()
        {
        }
    }
}