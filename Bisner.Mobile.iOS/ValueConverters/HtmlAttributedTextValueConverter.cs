using System;
using System.Globalization;
using Bisner.Mobile.iOS.Extensions;
using Foundation;
using MvvmCross.Platform.Converters;

namespace Bisner.Mobile.iOS.ValueConverters
{
    public class HtmlAttributedTextValueConverter : MvxValueConverter<string, NSAttributedString>
    {
        /// <summary>
        /// Fallback value to prevent binding from throwing nullref exceptions
        /// </summary>
        public static readonly NSAttributedString FallBackString = new NSAttributedString(string.Empty, Appearance.Fonts.LatoWithSize(10));

        protected override NSAttributedString Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return new NSAttributedString("");
            }

            // Build html attributed string
            NSError error = null;
            var htmlString = BuildHtml(value);
            var data = new NSString(htmlString).Encode(NSStringEncoding.Unicode);
            var attributedHtmlString = new NSAttributedString(data, new NSAttributedStringDocumentAttributes { DocumentType = NSDocumentType.HTML }, ref error);

            return attributedHtmlString;
        }

        private string BuildHtml(string value)
        {
            var hexString = Appearance.Colors.BisnerBlue.HexStringForColor();

            var html = string.Format(
                @"<html>
                                    <head>

                                        <style type='text/css'>
                                            body {{
                                                float: left;
                                                display: block;
                                                position: relative;
                                                font-family: 'Lato';
                                                font-size: 15px;
                                                color: #686868;
                                                margin: 0;
                                                padding: 0;
                                                width: 100%;
                                                line-height: 180%;
                                            }}

                                            .content {{
                                                float: left;
                                                display: block;
                                                position: relative;
                                                width: 100%;
                                                padding: 0 0 0 0;
                                            }}

                                            .content br{{
                                            float: left;
                                            clear: both;
                                            }}


                                            .content ul {{
                                            width: 100%;
                                            margin: 0;
                                            }}


                                            .content li {{
         
                                            }}

                                            .content a {{
                                                color: #{1};
                                                text-decoration: none;
                                            }}                                      
                                        </style>
                                    </head>


                                    <body>

                                        <div class=""content"">
                                            {0}
                                        </div>

                                    </body>
                                    </html>"
                , value, hexString);

            return html;
        }
    }
}
