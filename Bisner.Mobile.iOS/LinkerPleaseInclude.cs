using System.Collections.Specialized;
using System.Globalization;
using System.Windows.Input;
using Foundation;
using MvvmCross.Binding.iOS.Views.Gestures;
using MvvmCross.iOS.Views;
using MvvmCross.Platform.IoC;
using MvvmCross.Plugins.PictureChooser.iOS;
using UIKit;

namespace Bisner.Mobile.iOS
{
    // This class is never actually executed, but when Xamarin linking is enabled it does ensure types and properties
    // are preserved in the deployed app
    [Foundation.Preserve(AllMembers = true)]
    public class LinkerPleaseInclude
    {
        public void Include(UIButton uiButton)
        {
            uiButton.TouchUpInside += (s, e) =>
                                      uiButton.SetTitle(uiButton.Title(UIControlState.Normal), UIControlState.Normal);
            uiButton.Hidden = true;
        }

        public void Include(UIBarButtonItem barButton)
        {
            barButton.Clicked += (s, e) =>
                                 barButton.Title = barButton.Title + "";
            barButton.Enabled = false;
            barButton.TintColor = UIColor.Clear;
        }

        public void Include(UINavigationItem navigationItem)
        {
            navigationItem.LeftBarButtonItem.Enabled = false;
            navigationItem.RightBarButtonItem.Enabled = false;
        }

        public void Include(UITextField textField)
        {
            textField.Text = textField.Text + "";
            textField.EditingChanged += (sender, args) => { textField.Text = ""; };
            textField.Hidden = true;
            textField.AttributedPlaceholder = new NSAttributedString();
        }

        public void Include(UITextView textView)
        {
            textView.Text = textView.Text + "";
            textView.Changed += (sender, args) => { textView.Text = ""; };
            textView.Hidden = true;
            textView.AttributedText = new NSAttributedString("");
        }

        public void Include(UILabel label)
        {
            label.Text = label.Text + "";
            label.AttributedText = new NSAttributedString(label.AttributedText.ToString() + "");
            label.Tap();
        }

        public void Include(UIImageView imageView)
        {
            imageView.Image = new UIImage(imageView.Image.CGImage);
        }

        public void Include(UIDatePicker date)
        {
            date.Date = date.Date.AddSeconds(1);
            date.ValueChanged += (sender, args) => { date.Date = NSDate.DistantFuture; };
        }

        public void Include(UISlider slider)
        {
            slider.Value = slider.Value + 1;
            slider.ValueChanged += (sender, args) => { slider.Value = 1; };
        }

        public void Include(UIProgressView progress)
        {
            progress.Progress = progress.Progress + 1;
        }

        public void Include(UIActivityIndicatorView indicator)
        {
            indicator.Hidden = false;
        }

        public void Include(UISwitch sw)
        {
            sw.On = !sw.On;
            sw.ValueChanged += (sender, args) => { sw.On = false; };
        }

        public void Include(MvxViewController vc)
        {
            vc.Title = vc.Title + "";
        }

        public void Include(UIStepper s)
        {
            s.Value = s.Value + 1;
            s.ValueChanged += (sender, args) => { s.Value = 0; };
        }

        public void Include(UIPageControl s)
        {
            s.Pages = s.Pages + 1;
            s.ValueChanged += (sender, args) => { s.Pages = 0; };
        }

        public void Include(INotifyCollectionChanged changed)
        {
            changed.CollectionChanged += (s, e) => { var test = string.Format("{0}{1}{2}{3}{4}", e.Action, e.NewItems, e.NewStartingIndex, e.OldItems, e.OldStartingIndex); };
        }

        public void Include(ICommand command)
        {
            command.CanExecuteChanged += (s, e) => { if (command.CanExecute(null)) command.Execute(null); };
        }

        public void Include(MvxPropertyInjector injector)
        {
            injector = new MvxPropertyInjector();
        }

        public void Include(MvxInMemoryImageValueConverter converter)
        {
            converter.Convert(null, typeof(UIImage), null, CultureInfo.CurrentCulture);
        }

        public void Include(System.ComponentModel.INotifyPropertyChanged changed)
        {
            changed.PropertyChanged += (sender, e) => { var test = e.PropertyName; };
        }
    }
}

