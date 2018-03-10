using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Foundation;
using UIKit;

namespace Bisner.Mobile.iOS.Controls
{
    [Register("MyWebView")]
    public class FeedWebView : UIWebView
    {
        public FeedWebView()
        {
        }

        public FeedWebView(IntPtr handle)
            : base(handle)
        {
        }

        private string _htmlString;
        public string HtmlString
        {
            get { return _htmlString; }
            set
            {
                if (_htmlString == value) return;
                _htmlString = BuildHtml(value);
                LoadHtmlString(_htmlString, new NSUrl(Path.Combine(NSBundle.MainBundle.BundlePath, "Content/"), true));
            }
        }

        private string BuildHtml(string value)
        {
            return string.Format(
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
                                                color: #862C91;
                                            }}                                      
                                        </style>
                                    </head>


                                    <body>

                                        <div class=""content"">
                                            {0}
                                        </div>

                                    </body>
                                    </html>"
                                            , value);
        }
    }
}
