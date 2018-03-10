using Foundation;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace Bisner.Mobile.iOS.Views.ItemViews
{
    public class BodySliderCollectionViewSource : MvxCollectionViewSource
    {
        //private int lastSelectedIndex = 0;
        private readonly NSString cellIndentifier = new NSString("BodySliderItemView");
        //DashboardViewModel DashboardViewModel;
        private readonly bool _overlay;

        public BodySliderCollectionViewSource(UICollectionView collectionView, bool overlay = false)
            : base(collectionView)
        {
            //DashboardViewModel = viewModel;
            var xib = UINib.FromName("BodySliderItemView", null);
            collectionView.RegisterNibForCell(xib, cellIndentifier);
            //AddTeaColorViewModel.SelectItem(0, true);

            _overlay = overlay;
        }

        //public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        //{
        //	AddTeaColorViewModel.SelectItem(indexPath.Row);
        //	collectionView.DeselectItem(indexPath, true);
        //}

        protected override UICollectionViewCell GetOrCreateCellFor(UICollectionView collectionView, NSIndexPath indexPath, object item)
        {
            var cell = collectionView.DequeueReusableCell(cellIndentifier, indexPath) as BodySliderItemView;
            cell.InitStyle(_overlay);
            return cell;
        }
    }
}
