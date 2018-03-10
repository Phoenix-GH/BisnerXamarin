using System;
using CoreGraphics;
using UIKit;

namespace Bisner.Mobile.iOS.Views.ItemViews
{
    public partial class TimeSelectBlockView : UIView
    {
        public TimeSelectBlockView (IntPtr handle) : base (handle)
        {
        }

		public UIView VwBlcok
		{
			get
			{
				return this.vwBlock;
			}
		}

		public UIView VwResize
		{
			get
			{
				return this.vwResize;
			}
		}

		public void SetFrame(CGRect frame)
		{
			this.Frame = frame;
			//vwBlock.Frame = frame;
			//vwResize.Frame = new CGRect(0, vwBlock.Frame.Height - 10, vwBlock.Frame.Width, 10);
			//txvDescription.Frame = new CGRect(0, 0, vwBlock.Frame.Width, vwBlock.Frame.Height);
			this.LayoutIfNeeded();
		}

		bool _isShownIndicator = false;
		public bool IsShownIndicator
		{
			get
			{
				return _isShownIndicator;
			}

			set
			{
				_isShownIndicator = value;
				if (_isShownIndicator)
				{
					this.vwIndicator.Hidden = false;
				}
				else
				{
					this.vwIndicator.Hidden = true;
				}
			}
		}

		public void SetTag(int index)
		{
			vwBlock.Tag = index;
			vwResize.Tag = index;
			this.Tag = index;
		}

		public void InitStyle()
		{
			this.txvDescription.BackgroundColor = UIColor.FromRGB(253, 227, 215);
			this.txvDescription.Layer.BorderColor = UIColor.FromRGB(249, 173, 136).CGColor;
			this.txvDescription.Layer.BorderWidth = 1;
			this.txvDescription.Layer.CornerRadius = 4f;
			this.txvDescription.Layer.MasksToBounds = true;
			this.txvDescription.TextContainerInset = new UIEdgeInsets(2, 2, 2, 2);

			this.vwIndicator.Layer.BorderColor = UIColor.FromRGB(249, 173, 136).CGColor;
			this.vwIndicator.Layer.BorderWidth = 1;
			this.vwIndicator.BackgroundColor = UIColor.White;
			this.vwIndicator.Layer.MasksToBounds = true;
			this.vwIndicator.Layer.CornerRadius = this.vwIndicator.Frame.Height / 2;
			this.IsShownIndicator = false;
		}

        public void SetDescription(string description)
        {
            txvDescription.Text = description;
        }
    }
}