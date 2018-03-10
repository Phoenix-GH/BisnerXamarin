using System;
using System.Diagnostics;
using Bisner.Mobile.Core.Service;
using Foundation;
using MvvmCross.Platform;

namespace Bisner.Mobile.iOS.Extensions
{
    public static class NSstringExtensions
    {
        public static string EmojiToUnicode(this string value)
        {
            var data = new NSString(value).Encode(NSStringEncoding.NonLossyASCII);
            var goodValue = new NSString(data, NSStringEncoding.UTF8);

            return goodValue;
        }

        public static string ConvertEmoji(this string value)
        {
            var data = new NSString(value).Encode(NSStringEncoding.UTF8);
            var goodValue = new NSString(data, NSStringEncoding.NonLossyASCII);

            return goodValue;
        }

        #region Convert

        public static NSAttributedString ConvertHtml(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return new NSAttributedString("");
            }

            try
            {
                // Build html attributed string
                NSError error = null;
                var htmlString = BuildHtml(value);
                var data = new NSString(htmlString).Encode(NSStringEncoding.Unicode);
                var attributedHtmlString = new NSAttributedString(data,
                    new NSAttributedStringDocumentAttributes {DocumentType = NSDocumentType.HTML}, ref error);

                return attributedHtmlString;
            }
            catch (Exception ex)
            {
                Mvx.Resolve<IExceptionService>().HandleException(ex);
                return new NSAttributedString("");
            }
        }

        private static string BuildHtml(string value)
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

        #endregion Convert
    }
}