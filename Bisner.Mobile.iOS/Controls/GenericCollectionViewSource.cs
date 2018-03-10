using System;
using Bisner.Mobile.Core.Models.Base;
using Foundation;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace Bisner.Mobile.iOS.Controls
{
    public class GenericCollectionViewSource<TItem> : MvxCollectionViewSource where TItem : class, IItemBase
    {

        public GenericCollectionViewSource(UICollectionView collectionView) : base(collectionView)
        {
        }

        public GenericCollectionViewSource(UICollectionView collectionView, NSString defaultCellIdentifier) : base(collectionView, defaultCellIdentifier)
        {
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = base.GetCell(collectionView, indexPath);

            var item = GetItemAt(indexPath) as TItem;

            if (item != null)
            {
                if (ModifyCellFunc != null)
                {
                    ModifyCellFunc(collectionView, indexPath, cell, item);
                }
            }

            return cell;
        }

        public Action<UICollectionView, NSIndexPath, UICollectionViewCell, TItem> ModifyCellFunc { get; set; }
    }
}