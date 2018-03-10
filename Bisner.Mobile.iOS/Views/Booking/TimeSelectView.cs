using System;
using System.Collections.Generic;
using Bisner.Mobile.Core.Models.Booking;
using Bisner.Mobile.Core.ViewModels.Booking;
using Bisner.Mobile.iOS.Views.Base;
using Bisner.Mobile.iOS.Views.ItemViews;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Views;
using ObjCRuntime;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Booking
{
    [MvxFromStoryboard]
    public partial class TimeSelectView : ViewBase<TimeSelectViewModel>
    {
        private TimeSelectItemView[] _timeViews;    //TableVeiw items.
        private const float ItemHeight = 75f;
        private float _itemSlotHeight = ItemHeight;
        private CGRect _prevFrame;
        private List<TimeSelectBlockView> _blockViewList;   //Reserved Time Block List in the TimeSelectView

        public TimeSelectView(IntPtr handle) : base(handle)
        {
        }

        private TimeBlockType _timeBlockType;
        public TimeBlockType TimeBlockType
        {
            get => _timeBlockType;
            set
            {
                _timeBlockType = value;
                if (_timeBlockType == TimeBlockType.FIFTEEN)
                {
                    _itemSlotHeight = ItemHeight / 4;
                }
                else if (_timeBlockType == TimeBlockType.THIRTY)
                {
                    _itemSlotHeight = ItemHeight / 2;
                }
                else
                {
                    _itemSlotHeight = ItemHeight;
                }
            }
        }

        private TimeBlock _startEndTime;
        public TimeBlock StartEndTime
        {
            get => _startEndTime;
            set
            {
                _startEndTime = value;
                if (value != null)
                {
                    _timeViews = new TimeSelectItemView[StartEndTime.EndTime.Hour - StartEndTime.StartTime.Hour + 1];
                    vwContent.Frame = new CGRect(0, vwContent.Frame.Y, vwContent.Frame.Width,
                        ItemHeight * (StartEndTime.EndTime.Hour - StartEndTime.StartTime.Hour + 1));
                    for (var i = StartEndTime.StartTime.Hour; i < StartEndTime.EndTime.Hour + 1; i++)
                    {
                        var index = i - StartEndTime.StartTime.Hour;
                        var timeViewArray = NSBundle.MainBundle.LoadNib("TimeSelectItemView", this, null);
                        _timeViews[index] = Runtime.GetNSObject<UIView>(timeViewArray.ValueAt(0)) as TimeSelectItemView;
                        _timeViews[index].Frame = new CGRect(0, 75.0 * index, vwContent.Frame.Width, 75f);
                        var hour = ((i % 12) == 0) ? 12 : (i % 12);
                        _timeViews[index].Time = hour + ":00";
                        _timeViews[index].TimeBlockType = TimeBlockType;
                        vwContent.AddSubview(_timeViews[index]);
                    }
                    View.LayoutIfNeeded();
                }
            }
        }

        private MvxPropertyChangedListener _reservedTimeListListener;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _blockViewList = new List<TimeSelectBlockView>();
            var tapGesture = new UITapGestureRecognizer(TapContent);
            vwGestureBg.AddGestureRecognizer(tapGesture);

            var bindingSet = this.CreateBindingSet<TimeSelectView, TimeSelectViewModel>();
            bindingSet.Bind().For(c => c.TimeBlockType).To(vm => vm.TimeBlockType);
            bindingSet.Bind().For(c => c.StartEndTime).To(vm => vm.StartEndTime);
            //bindingSet.Bind().For(c => c.AlreadyReservedTimeList).To(vm => vm.AlreadyReservedTimeList);
            bindingSet.Bind(btnBack).To(vm => vm.BackBtnClickedCommand);
            bindingSet.Bind(btnBook).To(vm => vm.BookBtnClickedCommand);
            bindingSet.Apply();

            _reservedTimeListListener = new MvxPropertyChangedListener(ViewModel).Listen(() => ViewModel.AlreadyReservedTimeList, () =>
            {
                UpdateBlockViewList(ViewModel.AlreadyReservedTimeList);
            });

            InitStyle();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            if (IsMovingFromParentViewController)
            {
                _reservedTimeListListener.Dispose();
                _reservedTimeListListener = null;

                btnClose.TouchUpInside -= BtnCloseOnTouchUpInside;
            }
        }

        private void BtnCloseOnTouchUpInside(object sender, EventArgs eventArgs)
        {
            var previousController = NavigationController.ViewControllers[NavigationController.ViewControllers.Length - 2];
            previousController.RemoveFromParentViewController();
            NavigationController.PopViewController(true);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            NavigationController.NavigationBarHidden = true;

            if (IsMovingToParentViewController)
            {
                btnClose.TouchUpInside += BtnCloseOnTouchUpInside;
            }
        }

        public override bool PrefersStatusBarHidden()
        {
            return true;
        }

        public void InitStyle()
        {
            btnBook.Layer.CornerRadius = btnBook.Frame.Height / 2f;
            btnBook.Layer.MasksToBounds = true;

            btnBook.Font = Appearance.Fonts.LatoBoldWithSize(16);
        }

        public void UpdateBlockViewList(List<TimeBlock> blocks)
        {
            _blockViewList.Clear();
            foreach (var view in vwGestureBg.Subviews)
            {
                view.RemoveFromSuperview();
            }

            foreach (var item in blocks)
            {
                var startTime = item.StartTime;
                var endTime = item.EndTime;
                var y = (float)(item.StartTime.Hour * 60 + item.StartTime.Min - StartEndTime.StartTime.Hour * 60 - StartEndTime.StartTime.Min) / 60 * ItemHeight;
                var height = (float)(item.EndTime.Hour * 60 + item.EndTime.Min - item.StartTime.Hour * 60 - item.StartTime.Min) / 60 * ItemHeight;
                var frame = new CGRect(60, y, vwGestureBg.Frame.Width - 60, height);
                var block = CreateNewBlock(frame, false);
                block.SetDescription(item.ReservedByName);

            }

        }

        TimeSelectBlockView _newReservedBlockView;

        public TimeSelectBlockView CreateNewBlock(CGRect frame, bool hasGesture)
        {
            TimeSelectBlockView blockView;
            var viewArray = NSBundle.MainBundle.LoadNib("TimeSelectBlockView", this, null);

            if (hasGesture)
            {
                if (_newReservedBlockView == null)
                {
                    blockView = Runtime.GetNSObject<UIView>(viewArray.ValueAt(0)) as TimeSelectBlockView;
                    var panGesture = new UIPanGestureRecognizer(PanBlock);
                    blockView.VwBlcok.AddGestureRecognizer(panGesture);
                    var resizeGesture = new UIPanGestureRecognizer(ResizeBlock);
                    blockView.VwResize.AddGestureRecognizer(resizeGesture);
                    _newReservedBlockView = blockView;
                    _newReservedBlockView.SetTag(999);
                    vwGestureBg.AddSubview(blockView);
                    blockView.InitStyle();
                    blockView.SetFrame(frame);
                }
                else
                {
                    blockView = _newReservedBlockView;
                    var newFrame = new CGRect(frame.X, frame.Y, frame.Width, blockView.Frame.Height);
                    _newReservedBlockView.SetFrame(newFrame);
                }
            }
            else
            {
                blockView = Runtime.GetNSObject<UIView>(viewArray.ValueAt(0)) as TimeSelectBlockView;
                _blockViewList.Add(blockView);
                blockView.SetTag(_blockViewList.Count - 1);
                vwGestureBg.AddSubview(blockView);
                blockView.InitStyle();
                blockView.SetFrame(frame);
            }

            return blockView;
        }


        private void TapContent(UITapGestureRecognizer gestureRecognizer)
        {
            if (gestureRecognizer.State == UIGestureRecognizerState.Ended)
            {
                //var view = gestureRecognizer.View;
                var point = gestureRecognizer.LocationInView(vwGestureBg);
                foreach (var item in _blockViewList)
                    if (item.Frame.Contains(point)) return;
                foreach (var timeView in _timeViews)
                {
                    if (timeView.Frame.Contains(point))
                    {
                        var numbers = (int)(point.Y / _itemSlotHeight);
                        var frame = new CGRect(timeView.Frame.X + 60, numbers * _itemSlotHeight, timeView.Frame.Width - 60, _itemSlotHeight);
                        var blockView = CreateNewBlock(frame, true);
                        blockView.IsShownIndicator = true;
                        AdjustBlockFrameFromPan(blockView);
                        //view.LayoutIfNeeded();
                        var timeBlock = GetBlockTime(blockView.Frame);
                        ViewModel.NewReservedTime = timeBlock;
                    }
                }
            }

        }

        private void PanBlock(UIPanGestureRecognizer gestureRecognizer)
        {
            var view = gestureRecognizer.View;
            var index = (int)(view.Tag);
            try
            {
                var blockView = _newReservedBlockView;
                if (blockView == null) return;

                if (gestureRecognizer.State == UIGestureRecognizerState.Began)
                {
                    _prevFrame = new CGRect(blockView.Frame.X, blockView.Frame.Y, blockView.Frame.Width, blockView.Frame.Height);
                }
                else if (gestureRecognizer.State == UIGestureRecognizerState.Changed)
                {
                    var translation = gestureRecognizer.TranslationInView(blockView);
                    var frame = new CGRect(blockView.Frame.X, blockView.Frame.Y + translation.Y, blockView.Frame.Width, blockView.Frame.Height);
                    blockView.SetFrame(frame);
                    gestureRecognizer.SetTranslation(CGPoint.Empty, view);
                }
                else if (gestureRecognizer.State == UIGestureRecognizerState.Ended)
                {
                    for (var i = 0; i < _blockViewList.Count; i++)
                    {
                        if (i != index)
                        {
                            var compareTopY = _blockViewList[i].Frame.Top;
                            var compareBottomY = _blockViewList[i].Frame.Bottom;
                            if (blockView.Frame.Bottom > compareTopY && blockView.Frame.Top < compareBottomY)
                            {
                                blockView.Frame = _prevFrame;
                            }
                        }
                    }
                    AdjustBlockFrameFromPan(blockView);
                }
                var timeBlock = GetBlockTime(blockView.Frame);
                ViewModel.NewReservedTime = timeBlock;
            }
            catch (Exception e)
            {
            }

        }

        private void ResizeBlock(UIPanGestureRecognizer gestureRecognizer)
        {

            var view = gestureRecognizer.View;
            var index = (int)view.Tag;
            try
            {
                var blockView = _newReservedBlockView;
                if (blockView == null) return;
                if (gestureRecognizer.State == UIGestureRecognizerState.Began || gestureRecognizer.State == UIGestureRecognizerState.Changed)
                {
                    var translation = gestureRecognizer.TranslationInView(blockView);
                    var newHeight = blockView.Frame.Height + translation.Y;
                    newHeight = (newHeight < _itemSlotHeight) ? _itemSlotHeight : newHeight;
                    var bottompoint = new CGPoint(blockView.Frame.X, blockView.Frame.Y + newHeight);
                    for (var i = 0; i < _blockViewList.Count; i++)
                    {
                        if (i != index)
                        {
                            if (_blockViewList[i].Frame.Contains(bottompoint))
                            {
                                newHeight = _blockViewList[i].Frame.Top - blockView.Frame.Top;
                            }
                        }
                    }

                    var frame = new CGRect(blockView.Frame.X, blockView.Frame.Y, blockView.Frame.Width, newHeight);
                    blockView.SetFrame(frame);
                    gestureRecognizer.SetTranslation(CGPoint.Empty, view);
                }
                else if (gestureRecognizer.State == UIGestureRecognizerState.Ended)
                {
                    AdjustBlockFrameFromResize(blockView);
                }
                var timeBlock = GetBlockTime(blockView.Frame);
                ViewModel.NewReservedTime = timeBlock;
            }
            catch (Exception e)
            {
            }
        }

        private void UpdateBlockFrame(CGRect frame)
        {

            //vwBlock.Frame = frame;
            //vwResize.Frame = new CGRect(0, vwBlock.Frame.Height - 10, vwBlock.Frame.Width, 10);
            //txvDescription.Frame = new CGRect(0, 0, vwBlock.Frame.Width, vwBlock.Frame.Height);
        }

        private void AdjustBlockFrameFromPan(TimeSelectBlockView viewBlock)
        {
            var originFrame = viewBlock.Frame;
            var numbers = originFrame.Y / _itemSlotHeight;
            var rounds = (int)(Math.Round(numbers));
            var roundedFrame = new CGRect(originFrame.X, _itemSlotHeight * rounds, originFrame.Width, originFrame.Height);
            var newFrame = roundedFrame;
            if (roundedFrame.Y < 0)
            {
                newFrame = new CGRect(roundedFrame.X, 0, roundedFrame.Width, roundedFrame.Height);
            }
            if (roundedFrame.Y + roundedFrame.Height > vwGestureBg.Frame.Height)
            {
                newFrame = new CGRect(roundedFrame.X, vwGestureBg.Frame.Height - roundedFrame.Height, roundedFrame.Width, roundedFrame.Height);
            }
            viewBlock.SetFrame(newFrame);
        }

        private void AdjustBlockFrameFromResize(TimeSelectBlockView viewBlock)
        {
            var originFrame = viewBlock.Frame;
            if (originFrame.Height < _itemSlotHeight)
                originFrame = new CGRect(viewBlock.Frame.X, viewBlock.Frame.Y, viewBlock.Frame.Width, _itemSlotHeight);
            var numbers = originFrame.Height / _itemSlotHeight;
            var rounds = (int)Math.Round(numbers);
            var roundedFrame = new CGRect(originFrame.X, originFrame.Y, originFrame.Width, _itemSlotHeight * rounds);
            var newFrame = roundedFrame;
            if (roundedFrame.Y < 0)
            {
                newFrame.Y = 0;
            }
            if (roundedFrame.Y + roundedFrame.Height > vwGestureBg.Frame.Height)
            {
                newFrame = new CGRect(roundedFrame.X, roundedFrame.Y, roundedFrame.Width, vwGestureBg.Frame.Height - roundedFrame.Y);
            }
            viewBlock.SetFrame(newFrame);
        }

        private TimeBlock GetBlockTime(CGRect frame)
        {
            var y = frame.Y;
            var hours = (int)(y / ItemHeight) + StartEndTime.StartTime.Hour;
            var tempMins = (y % ItemHeight) * 60 / ItemHeight;
            var mins = (int)(tempMins / 15) * 15;
            var startTime = new TimePickerData(hours, mins);

            y = frame.Y + frame.Height;
            hours = (int)(y / ItemHeight) + StartEndTime.StartTime.Hour;
            tempMins = y % ItemHeight * 60 / ItemHeight;
            mins = (int)(tempMins / 15) * 15;
            var endTime = new TimePickerData(hours, mins);
            return new TimeBlock(startTime, endTime);
        }

        protected override bool EnableCustomBackButton => true;
    }
}