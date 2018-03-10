using Bisner.Mobile.Core.ViewModels.Booking;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace Bisner.Mobile.iOS.Views.ItemViews
{
	public class TimeLineSliderCollectionViewSource : MvxCollectionViewSource
	{
	    private NSString cellIndentifier = new NSString("TimeLineItemView");
	    private RoomTimeIndexItemView RoomTimeIndexItemView;

	    public TimeLineSliderCollectionViewSource(UICollectionView collectionView, RoomTimeIndexItemView view)
	        : base(collectionView)
	    {
	        RoomTimeIndexItemView = view;
	        var xib = UINib.FromName("TimeLineItemView", null);
	        collectionView.RegisterNibForCell(xib, cellIndentifier);
	    }

	    protected override UICollectionViewCell GetOrCreateCellFor(UICollectionView collectionView, NSIndexPath indexPath, object item)
	    {
	        var cell = collectionView.DequeueReusableCell(cellIndentifier, indexPath) as TimeLineItemView;
	        var bindingSet = cell.CreateBindingSet<TimeLineItemView, TimeLineItemViewModel>();
	        bindingSet.Bind().For(c => c.TimeString).To(vm => vm.TimeString);
	        bindingSet.Bind().For(c => c.TimeBlockType).To(vm => vm.TimeBlockType);
	        bindingSet.Apply();
	        cell.InitStyle();
	        return cell;
	    }
    }
}
