using System;
using Bisner.Mobile.Core.ViewModels;
using MvvmCross.iOS.Views;

namespace Bisner.Mobile.iOS.Views
{
	partial class GalleryView : MvxPageViewController<GalleryViewModel>
	{
		public GalleryView (IntPtr handle) : base (handle)
		{
		}
	}
}
