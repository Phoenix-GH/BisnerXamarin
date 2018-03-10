﻿using System;
using CoreAnimation;
using CoreGraphics;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Layout
{
	public class LineLayoutForBodySlider : UICollectionViewFlowLayout
	{
		public const float ITEM_HEIGHT = 90f;
		public const float ITEM_WIDTH = 90f;
		public const int ACTIVE_DISTANCE = 0;
		public const float ZOOM_FACTOR = 0f;

		public LineLayoutForBodySlider()
		{
			ItemSize = new CGSize(ITEM_WIDTH, ITEM_HEIGHT);
			ScrollDirection = UICollectionViewScrollDirection.Horizontal;
			//TODO is should be revised for iPad.
			//SectionInset = (AppDelegate.IsIPad) ? new UIEdgeInsets(0, 60f, 0, 60f) : new UIEdgeInsets(0, 20f, 0, 20f);
			SectionInset = new UIEdgeInsets(10f, 10f, 10f, 10f);
			MinimumLineSpacing = 10.0f;
		}

		public override bool ShouldInvalidateLayoutForBoundsChange(CGRect newBounds)
		{
			return true;
		}

		public override UICollectionViewLayoutAttributes[] LayoutAttributesForElementsInRect(CGRect rect)
		{
			var array = base.LayoutAttributesForElementsInRect(rect);
			var visibleRect = new CGRect(CollectionView.ContentOffset, CollectionView.Bounds.Size);

			foreach (var attributes in array)
			{
				if (attributes.Frame.IntersectsWith(rect))
				{
					float distance = (float)(visibleRect.GetMidX() - attributes.Center.X);
					float normalizedDistance = distance / ACTIVE_DISTANCE;
					if (Math.Abs(distance) < ACTIVE_DISTANCE)
					{
						float zoom = 1 + ZOOM_FACTOR * (1 - Math.Abs(normalizedDistance));
						attributes.Transform3D = CATransform3D.MakeScale(zoom, zoom, 1.0f);
						attributes.ZIndex = 1;
					}
				}
			}
			return array;
		}

		public override CGPoint TargetContentOffset(CGPoint proposedContentOffset, CGPoint scrollingVelocity)
		{
			float offSetAdjustment = float.MaxValue;
			float horizontalCenter = (float)(proposedContentOffset.X + (this.CollectionView.Bounds.Size.Width / 2.0));
			CGRect targetRect = new CGRect(proposedContentOffset.X, 0.0f, this.CollectionView.Bounds.Size.Width, this.CollectionView.Bounds.Size.Height);
			var array = base.LayoutAttributesForElementsInRect(targetRect);
			foreach (var layoutAttributes in array)
			{
				float itemHorizontalCenter = (float)layoutAttributes.Center.X;
				if (Math.Abs(itemHorizontalCenter - horizontalCenter) < Math.Abs(offSetAdjustment))
				{
					offSetAdjustment = itemHorizontalCenter - horizontalCenter;
				}
			}
			return new CGPoint(proposedContentOffset.X + offSetAdjustment, proposedContentOffset.Y);
		}

	}
}
