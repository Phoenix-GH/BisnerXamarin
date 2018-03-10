using System.Collections.Generic;
using Bisner.Mobile.Core.Models.Base;

namespace Bisner.Mobile.Core.Helpers
{
    public class DistinctItemBaseComparer<T> : IEqualityComparer<T> where T : IItemBase
    {
        public bool Equals(T x, T y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(T obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
