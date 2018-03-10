using System;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.Models.Base
{
    public abstract class ItemBase : MvxNavigatingObject, IItemBase
    {
        private Guid _id;

        public Guid Id
        {
            get => _id;
            set
            {
                _id = value;
                RaisePropertyChanged(() => Id);
            }
        }
    }
}