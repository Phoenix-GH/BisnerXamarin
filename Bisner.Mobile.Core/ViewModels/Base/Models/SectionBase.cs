using System.Collections.Generic;

namespace Bisner.Mobile.Core.ViewModels.Base.Models
{
    public abstract class SectionBase<T> : List<T>
    {
        public string Name { get; set; }

        protected SectionBase()
        {

        }

        protected SectionBase(IEnumerable<T> items)
        {
            AddRange(items);
        }
    }
}