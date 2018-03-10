using System.Text.RegularExpressions;

namespace Bisner.Mobile.Core.Helpers
{
    public static class RegexHelper
    {
        public static string MatchReplace(string pattern, string match, string content)
        {
            return MatchReplace(pattern, match, content, false, false, false);
        }

        public static string MatchReplace(string pattern, string match, string content, bool multi)
        {
            return MatchReplace(pattern, match, content, multi, false, false);
        }

        public static string MatchReplace(string pattern, string match, string content, bool multi, bool white)
        {
            return MatchReplace(pattern, match, content, multi, white);
        }

        public static string MatchReplace(string pattern, string match, string content, bool multi, bool white, bool cult)
        {
            if (multi && white && cult)
                return Regex.Replace(content, pattern, match, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            if (multi && white)
                return Regex.Replace(content, pattern, match, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnoreCase);
            if (multi && cult)
                return Regex.Replace(content, pattern, match, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.CultureInvariant);
            if (white && cult)
                return Regex.Replace(content, pattern, match, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.CultureInvariant);
            if (multi)
                return Regex.Replace(content, pattern, match, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if (white)
                return Regex.Replace(content, pattern, match, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
            if (cult)
                return Regex.Replace(content, pattern, match, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            // Default
            return Regex.Replace(content, pattern, match, RegexOptions.IgnoreCase);
        }
    }
}