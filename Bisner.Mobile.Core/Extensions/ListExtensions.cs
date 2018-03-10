using System;
using System.Collections.Generic;
using System.Text;

namespace Bisner.Mobile.Core.Extensions
{
    public static class ListExtensions
    {
        public static string ToGuidString(this List<Guid> value)
        {
            if (value == null)
            {
                return null;
            }

            var builder = new StringBuilder();

            foreach (var guid in value)
            {
                builder.Append(guid + ";");
            }

            return builder.ToString();
        }

        public static List<Guid> ToGuidList(this string guidString)
        {
            if (guidString == null)
                return new List<Guid>();

            var strings = guidString.Split(';');

            var ids = new List<Guid>();

            foreach (var s in strings)
            {
                Guid id;

                if (Guid.TryParse(s, out id))
                {
                    ids.Add(id);
                }
            }

            return ids;
        }
    }
}