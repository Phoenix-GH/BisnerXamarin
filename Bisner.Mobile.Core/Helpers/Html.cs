namespace Bisner.Mobile.Core.Helpers
{
    public static class Html
    {
        public static string StripTags(string content)
        {
            return string.IsNullOrEmpty(content) ? null : RegexHelper.MatchReplace(@"< [^>]+>", "", content, true, true, true);
        }

        public static string RemoveSpecialChars(string content)
        {
            return string.IsNullOrEmpty(content) ? null : content.Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");
        }

        public static string ReturnSpecialChars(string content)
        {
            return string.IsNullOrEmpty(content) ? null : content.Replace("&lt;", "<").Replace("&gt;", ">").Replace("&quot;", "\"");
        }
    }
}