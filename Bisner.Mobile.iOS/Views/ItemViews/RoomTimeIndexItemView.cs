using System;
using System.Collections.Generic;
using System.Windows.Input;
using Bisner.Mobile.Core.Models.Booking;
using Bisner.Mobile.Core.ViewModels.Booking;
using Bisner.Mobile.iOS.Views.Layout;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Binding.iOS.Views.Gestures;
using UIKit;

namespace Bisner.Mobile.iOS.Views.ItemViews
{
    public partial class RoomTimeIndexItemView : MvxTableViewCell
    {
        public static NSString Identifier = new NSString("RoomTimeIndexItemView");

        const float ITEM_WIDTH = 75f;
        public RoomTimeIndexItemView(IntPtr handle) : base(handle)
        {
            InitBinding();
        }

        public TimeBlockType TimeBlockType { get; set; }

        public TimeBlock StartEndTime { get; set; }

        List<TimeBlock> _blockTimeList;
        public List<TimeBlock> BlockTimeList
        {
            get => _blockTimeList;
            set
            {
                _blockTimeList = value;
                if (StartEndTime != null && _blockTimeList != null)
                    AddTimeBlock();
            }
        }



        public void InitStyle()
        {
            lblTitle.Font = iOS.Appearance.Fonts.LatoBlackWithSize(20);
        }

        public void InitBinding()
        {
            this.DelayBind(() =>
            {
                var bodySliderSource = new TimeLineSliderCollectionViewSource(clvTimeLine, this);
                clvTimeLine.SetCollectionViewLayout(new LineLayoutForTimeLineSlider(), true);
                clvTimeLine.Delegate = new UICollectionViewDelegateFlowLayout();
                clvTimeLine.Source = bodySliderSource;
                clvTimeLine.ReloadData();

                var set = this.CreateBindingSet<RoomTimeIndexItemView, RoomTimeIndexItemViewModel>();
                set.Bind(ivContent).For("ImageUrl").To(vm => vm.ImageUrl).WithConversion("ImageUrl");
                set.Bind(ivContent.Tap()).For(c => c.Command).To(vm => vm.ShowRoomCommand);
                set.Bind(lblTitle).To(vm => vm.Title);
                set.Bind(vwStatus).For("AvailableBackground").To(vm => vm.IsAvailable);
                set.Bind(this).For(t => t.TimeBlockType).To(vm => vm.TimeBlockType);
                set.Bind(this).For(t => t.StartEndTime).To(vm => vm.StartEndTime);
                set.Bind(this).For(t => t.BlockTimeList).To(vm => vm.BlockTimeList);
                set.Bind(bodySliderSource).For(i => i.ItemsSource).To(vm => vm.TimeLineList);
                set.Apply();
            });

        }

        void AddTimeBlock()
        {
            foreach (var subview in clvTimeLine.Subviews)
            {
                if (subview is UITextView)
                {
                    subview.RemoveFromSuperview();
                }
            }

            foreach (var blockTime in BlockTimeList)
            {
                var textView = new UITextView
                {
                    Editable = false,
                    Text = blockTime.ReservedByName,
                    TextColor = UIColor.FromRGB(245, 119, 59),
                    BackgroundColor = UIColor.FromRGB(253, 227, 215)
                };
                textView.Layer.BorderColor = UIColor.FromRGB(249, 173, 136).CGColor;
                textView.Layer.BorderWidth = 1;
                textView.Layer.CornerRadius = 4f;
                textView.Layer.MasksToBounds = true;
                var startX = (blockTime.StartTime.Hour * 60 + blockTime.StartTime.Min - StartEndTime.StartTime.Hour * 60 - StartEndTime.StartTime.Min) * ITEM_WIDTH / 60;
                var endX = (blockTime.EndTime.Hour * 60 + blockTime.EndTime.Min - StartEndTime.StartTime.Hour * 60 - StartEndTime.StartTime.Min) * ITEM_WIDTH / 60;
                textView.Frame = new CGRect(startX, 25, endX - startX, clvTimeLine.Frame.Height - 27);
                clvTimeLine.AddSubview(textView);
            }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            ivContent.Transparency = 0.5f;
        }
    }
}