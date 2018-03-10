namespace Bisner.Mobile.Core.Helpers
{
    public class BbCode
    {
        public static string ConvertToHtml(string content)
        {
            // Remove all < > from text (DO NOT ALLOW HTML IN THE INPUT TEXT)
            content = Html.StripTags(content);

            content = RegexHelper.MatchReplace(@"\[b\]([^\]]+)\[\/b\]", "<strong>$1</strong>", content);
            //content = RegexHelper.MatchReplace(@"/\*(\S(.*?\S)?)\*/gm", "<strong>$1</strong>", content);
            content = RegexHelper.MatchReplace(@"\*(.[^*]*)\*", "<strong>$1</strong>", content);

            //content = RegexHelper.MatchReplace(@"\[center\](.+)\[\/center\]", "<center>$1</center>", content);
            content = RegexHelper.MatchReplace(@"\[i\]([^\]]+)\[\/i\]", "<i>$1</i>", content);
            content = RegexHelper.MatchReplace(@"\[i\]([^\]]+)\[\/i\]", "<i>$1</i>", content);
            content = RegexHelper.MatchReplace(@"\[u\]([^\]]+)\[\/u\]", "<span style=\"text-decoration:underline\">$1</span>", content);
            //content = RegexHelper.MatchReplace(@"\[del\]([^\]]+)\[\/del\]", "<span style=\"text-decoration:line-through\">$1</span>", content);

            // Colors and sizes
            //content = RegexHelper.MatchReplace(@"\[color=(#[0-9a-fA-F]{6}|[a-z-]+)]([^\]]+)\[\/color\]", "<span style=\"color:$1;\">$2</span>", content);
            //content = RegexHelper.MatchReplace(@"\[size=([2-5])]([^\]]+)\[\/size\]", "<span style=\"font-size:$1em; font-weight:normal;\">$2</span>", content);

            // Text alignment
            //content = RegexHelper.MatchReplace(@"\[left\]([^\]]+)\[\/left\]", "<span style=\"text-align:left\">$1</span>", content);
            //content = RegexHelper.MatchReplace(@"\[right\]([^\]]+)\[\/right\]", "<span style=\"text-align:right\">$1</span>", content);
            //content = RegexHelper.MatchReplace(@"\[center\]([^\]]+)\[\/center\]", "<span style=\"text-align:center\">$1</span>", content);
            //content = RegexHelper.MatchReplace(@"\[justify\]([^\]]+)\[\/justify\]", "<span style=\"text-align:justify\">$1</span>", content);

            // HTML Links
            //content = RegexHelper.MatchReplace(@"\[url\]([^\]]+)\[\/url\]", "<a href=\"$1\">$1</a>", content);
            content = RegexHelper.MatchReplace(@"\[url=([^\]]+)]([^\]]+)\[\/url\]", "<a href=\"$1\">$2</a>", content);
            content = RegexHelper.MatchReplace(@"\[mention=([^\]]+)]([^\]]+)\[\/mention\]", "<a href=\"mention://$1\">$2</a>", content);

            // Images
            //content = RegexHelper.MatchReplace(@"\[img\]([^\]]+)\[\/img\]", "<img src=\"$1\" alt=\"\" />", content);
            //content = RegexHelper.MatchReplace(@"\[img=([^\]]+)]([^\]]+)\[\/img\]", "<img src=\"$2\" alt=\"$1\" />", content);

            // Lists
            //content = RegexHelper.MatchReplace(@"\[*\]([^\]+)", "<li>$1</li>", content);
            //content = RegexHelper.MatchReplace(@"\[list\]([^\]]+)\[\/list\]", "<ul>$1</ul><p>", content);
            //content = RegexHelper.MatchReplace(@"\[list=1\]([^\]]+)\[\/list\]", "</p><ol>$1</ol><p>", content);

            // Headers
            content = RegexHelper.MatchReplace(@"\[h1\]([^\]]+)\[\/h1\]", "<h1>$1</h1>", content);
            content = RegexHelper.MatchReplace(@"\[h2\]([^\]]+)\[\/h2\]", "<h2>$1</h2>", content);
            content = RegexHelper.MatchReplace(@"\[h3\]([^\]]+)\[\/h3\]", "<h3>$1</h3>", content);
            content = RegexHelper.MatchReplace(@"\[h4\]([^\]]+)\[\/h4\]", "<h4>$1</h4>", content);
            content = RegexHelper.MatchReplace(@"\[h5\]([^\]]+)\[\/h5\]", "<h5>$1</h5>", content);
            content = RegexHelper.MatchReplace(@"\[h6\]([^\]]+)\[\/h6\]", "<h6>$1</h6>", content);

            // Horizontal rule
            content = RegexHelper.MatchReplace(@"\[hr\]", "<hr />", content);

            // Set a maximum quote depth (In this case, hard coded to 3)
            for (int i = 1; i < 3; i++)
            {
                // Quotes
                //content = MatchReplace(@"\[quote=([^\]]+)@([^\]]+)|([^\]]+)]([^\]]+)\[\/quote\]", "</p><div class=\"block\"><blockquote><cite>$1 <a href=\"" + QuoteUrl("$3") + "\">wrote</a> on $2</cite><hr /><p>$4</p></blockquote></div></p><p>", content);
                content = RegexHelper.MatchReplace(@"\[quote=([^\]]+)@([^\]]+)]([^\]]+)\[\/quote\]", "</p><div class=\"block\"><blockquote><cite>$1 wrote on $2</cite><hr /><p>$3</p></blockquote></div><p>", content);
                content = RegexHelper.MatchReplace(@"\[quote=([^\]]+)]([^\]]+)\[\/quote\]", "</p><div class=\"block\"><blockquote><cite>$1 wrote</cite><hr /><p>$2</p></blockquote></div><p>", content);
                content = RegexHelper.MatchReplace(@"\[quote\]([^\]]+)\[\/quote\]", "</p><div class=\"block\"><blockquote><p>$1</p></blockquote></div><p>", content);
            }

            // The following markup is for embedded video -->   

            // YouTube
            content = RegexHelper.MatchReplace(@"\[youtube\]http:\/\/([a-zA-Z]+.)youtube.com\/watch\?v=([a-zA-Z0-9_\-]+)\[\/youtube\]",
                "<center><object width=\"425\" height=\"344\"><param name=\"movie\" value=\"http://www.youtube.com/v/$2\"></param><param name=\"allowFullScreen\" value=\"true\"></param><embed src=\"http://www.youtube.com/v/$2\" type=\"application/x-shockwave-flash\" allowfullscreen=\"true\" width=\"425\" height=\"344\"></embed></object></center>", content);

            // LiveVideo
            content = RegexHelper.MatchReplace(@"\[livevideo\]http:\/\/([a-zA-Z]+.)livevideo.com\/video\/([a-zA-Z0-9_\-]+)\/([a-zA-Z0-9]+)\/([a-zA-Z0-9_\-]+).aspx\[\/livevideo\]",
                "<center><object width=\"445\" height=\"369\"><embed src=\"http://www.livevideo.com/flvplayer/embed/$3\" type=\"application/x-shockwave-flash\" quality=\"high\" width=\"445\" height=\"369\" wmode=\"transparent\"></embed></object></center>", content);

            // LiveVideo (There are two types of links for LV)
            content = RegexHelper.MatchReplace(@"\[livevideo\]http:\/\/([a-zA-Z]+.)livevideo.com\/video\/([a-zA-Z0-9]+)\/([a-zA-Z0-9_\-]+).aspx\[\/livevideo\]",
                "<center><object width=\"445\" height=\"369\"><embed src=\"http://www.livevideo.com/flvplayer/embed/$2&autostart=0\" type=\"application/x-shockwave-flash\" quality=\"high\" width=\"445\" height=\"369\" wmode=\"transparent\"></embed></object></center>", content);

            // Metacafe
            content = RegexHelper.MatchReplace(@"\[metacafe\]http\:\/\/([a-zA-Z]+.)metacafe.com\/watch\/([0-9]+)\/([a-zA-Z0-9_]+)/\[\/metacafe\]",
                "<center><object width=\"400\" height=\"345\"><embed src=\"http://www.metacafe.com/fplayer/$2/$3.swf\" width=\"400\" height=\"345\" wmode=\"transparent\" pluginspage=\"http://www.macromedia.com/go/getflashplayer\" type=\"application/x-shockwave-flash\"></embed></object></center>", content);

            // LiveLeak
            content = RegexHelper.MatchReplace(@"\[liveleak\]http:\/\/([a-zA-Z]+.)liveleak.com\/view\?i=([a-zA-Z0-9_]+)\[\/liveleak\]",
                "<center><object width=\"450\" height=\"370\"><param name=\"movie\" value=\"http://www.liveleak.com/e/$2\"></param><param name=\"wmode\" value=\"transparent\"></param><embed src=\"http://www.liveleak.com/e/$2\" type=\"application/x-shockwave-flash\" wmode=\"transparent\" width=\"450\" height=\"370\"></embed></object></center>", content);

            // < -- End video markup   

            content = RegexHelper.MatchReplace(@"\[google\]([^\]]+)\[\/google\]", "<a href=\"http://www.google.com/search?q=$1\" target=\"_BLANK\">$1</a>", content);
            content = RegexHelper.MatchReplace(@"\[bing\]([^\]]+)\[\/bing\]", "<a href=\"http://www.bing.com/search?q=$1\" target=\"_BLANK\">$1</a>", content);
            content = RegexHelper.MatchReplace(@"\[wikipedia\]([^\]]+)\[\/wikipedia\]", "<a href=\"http://www.wikipedia.org/wiki/$1\" target=\"_BLANK\">$1</a>", content);

            // Put the content in a paragraph
            //content = "<p>" + content + "</p>";   

            // Clean up a few potential markup problems
            content = content.Replace("\t", "    ")
                .Replace("  ", "  ")
                .Replace("<p><br />", "</p><p>")
                .Replace("</p><p><blockquote>", "<blockquote>")
                .Replace("</blockquote></blockquote></p>", "")
                .Replace("<p></p>", "")
                .Replace("<p><ul></ul></p>", "<ul>")
                .Replace("<p></p></ul>", "")
                .Replace("<p><ol></ol></p>", "<ol>")
                .Replace("<p></p></ol>", "")
                .Replace("<p><li>", "</li><li><p>")
                .Replace("</p></li></p>", "")
                .Replace("\r\n", "<br />")
                .Replace("\n\n", "<p></p>");

            return content;
        }
    }
}