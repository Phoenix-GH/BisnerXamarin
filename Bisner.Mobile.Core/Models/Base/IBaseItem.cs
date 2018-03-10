using System;

namespace Bisner.Mobile.Core.Models.Base
{
    public interface IItemBase
    {
        Guid Id { get; set; }
    }

    public interface IBusyItemBase : IItemBase
    {
        bool IsBusy { get; }

        bool IsNotBusy { get; }
    }

    public interface INamed
    {
        string Name { get; set; }
    }
}
