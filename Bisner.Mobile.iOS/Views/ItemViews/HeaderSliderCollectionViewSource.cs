using Bisner.Mobile.Core.ViewModels.Booking;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace Bisner.Mobile.iOS.Views.ItemViews
{
	public class HeaderSliderCollectionViewSource : MvxCollectionViewSource
	{
		//private int lastSelectedIndex = 0;
		private readonly NSString _cellIndentifier = new NSString("HeaderSliderItemView");
		//DashboardViewModel DashboardViewModel;

		public HeaderSliderCollectionViewSource(UICollectionView collectionView)
			: base(collectionView)
		{
			//DashboardViewModel = viewModel;
			var xib = UINib.FromName("HeaderSliderItemView", null);
			collectionView.RegisterNibForCell(xib, _cellIndentifier);
			//AddTeaColorViewModel.SelectItem(0, true);
		}

		//public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
		//{
		//	AddTeaColorViewModel.SelectItem(indexPath.Row);
		//	collectionView.DeselectItem(indexPath, true);
		//}

		protected override UICollectionViewCell GetOrCreateCellFor(UICollectionView collectionView, NSIndexPath indexPath, object item)
		{
			var cell = collectionView.DequeueReusableCell(_cellIndentifier, indexPath) as HeaderSliderItemView;
			cell.InitStyle();
			return cell;
		}

	}
}
