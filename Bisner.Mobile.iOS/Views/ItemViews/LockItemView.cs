using System;
using Bisner.Mobile.Core.ViewModels.AccessControl;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Core.ViewModels;
using UIKit;

namespace Bisner.Mobile.iOS.Views.ItemViews
{
    public partial class LockItemView : MvxTableViewCell
    {
        public static readonly NSString Identifier = new NSString("LockItemView");

        protected LockItemView(IntPtr handle) : base(handle)
        {
            SetupBindings();
        }

        public void SetupStyle()
        {
            ContentView.BackgroundColor = UIColor.Clear;
            BackgroundColor = UIColor.Clear;

            BackPanel.Layer.CornerRadius = 5f;
            BackPanel.Layer.BorderColor = iOS.Appearance.Colors.BackPanelBorderTop.CGColor;
            BackPanel.Layer.BorderWidth = 1f;

            TitleLabel.Font = iOS.Appearance.Fonts.LatoWithSize(20);
            TitleLabel.TextColor = iOS.Appearance.Colors.DefaultTextColor;
            TitleLabel.BackgroundColor = UIColor.Clear;
            SubTitleLabel.Font = iOS.Appearance.Fonts.LatoWithSize(14);
            SubTitleLabel.TextColor = iOS.Appearance.Colors.SubTextColor;
            SubTitleLabel.BackgroundColor = UIColor.Clear;

            TextContainer.BackgroundColor = UIColor.Clear;

            Image.ClipsToBounds = true;
            Image.ContentMode = UIViewContentMode.ScaleAspectFill;
        }

        private MvxPropertyChangedListener _stateListener;

        private AccessControlItemViewModel ViewModel
        {
            get
            {
                var model = BindingContext.DataContext as AccessControlItemViewModel;

                return model;
            }
        }

        public void SetupBindings()
        {
            this.DelayBind(() =>
            {
                _stateListener = new MvxPropertyChangedListener(ViewModel).Listen(() => ViewModel.State, () =>
                {
                    BeginInvokeOnMainThread(() =>
                    {
                        UIImage image;

                        if (ViewModel.State == LockState.Open)
                        {
                            image = UIImage.FromFile("Icons/doorlock_icon_003.png").ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);

                            TitleLabel.TextColor = UIColor.White;
                            TitleLabel.TextColor = UIColor.White;
                            Image.Image = image;
                        }
                        else if (ViewModel.State == LockState.Opening)
                        {
                            image = UIImage.FromFile("Icons/doorlock_icon_002.png").ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
                            
                            Image.Image = image;
                        }
                        else
                        {
                            image = UIImage.FromFile("Icons/doorlock_icon_001.png").ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);

                            TitleLabel.TextColor = iOS.Appearance.Colors.DefaultTextColor;
                            SubTitleLabel.TextColor = iOS.Appearance.Colors.SubTextColor;
                        }

                        Image.Image = image;
                    });
                });

                var set = this.CreateBindingSet<LockItemView, AccessControlItemViewModel>();
                set.Bind(TitleLabel).To(vm => vm.Title);
                set.Bind(SubTitleLabel).To(vm => vm.SubTitle);
                set.Bind(BackPanel).For("AccessControlBackground").To(vm => vm.State);
                set.Bind(BackPanel).For("AccessControlBorder").To(vm => vm.State);
                //set.Bind(ImageView).For("AccessControlImage").To(vm => vm.State);
                set.Bind(LockButton).To(vm => vm.OpenCommand);
                set.Apply();
            });
        }
    }
}
