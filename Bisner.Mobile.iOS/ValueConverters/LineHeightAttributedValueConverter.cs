using System;
using System.Globalization;
using Foundation;
using MvvmCross.Platform.Converters;
using UIKit;

namespace Bisner.Mobile.iOS.ValueConverters
{
    public class LineHeightAttributedValueConverter : MvxValueConverter<string, NSAttributedString>
    {
        protected override NSAttributedString Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
            var labelString = new NSMutableAttributedString(value);
            var paragraphStyle = new NSMutableParagraphStyle { LineSpacing = 4 };
            var style = UIStringAttributeKey.ParagraphStyle;
            var range = new NSRange(0, labelString.Length);

            labelString.AddAttribute(style, paragraphStyle, range);

            return labelString;
        }
    }
}