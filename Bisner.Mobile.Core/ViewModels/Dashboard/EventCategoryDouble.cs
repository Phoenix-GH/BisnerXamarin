using System;
using Bisner.Mobile.Core.Models.Base;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.ViewModels.Dashboard
{
    public class EventCategoryDouble : ItemBase
    {
        private Guid _id1;
        private string _title1;
        private string _image1;
        private Guid _id2;
        private string _title2;
        private string _image2;

        public Guid Id1
        {
            get => _id1;
            set { _id1 = value; RaisePropertyChanged(() => Id1); }
        }

        public string Title1
        {
            get => _title1;
            set { _title1 = value; RaisePropertyChanged(() => Title1); }
        }

        public string Image1
        {
            get => _image1;
            set
            {
                _image1 = value;
                RaisePropertyChanged(() => Image1);
            }
        }

        private MvxCommand _image1Command;

        public MvxCommand Image1Command
        {
            get
            {
                return _image1Command ?? (_image1Command = new MvxCommand(() =>
                {
                    Image1Action?.Invoke();
                }));
            }
        }

        public Action Image1Action { get; set; }

        public Guid Id2
        {
            get => _id2;
            set { _id2 = value; RaisePropertyChanged(() => Id2); }
        }

        public string Title2
        {
            get => _title2;
            set { _title2 = value; RaisePropertyChanged(() => Title2); }
        }

        public string Image2
        {
            get => _image2;
            set { _image2 = value; RaisePropertyChanged(() => Image2); }
        }

        private MvxCommand _image2Command;

        public MvxCommand Image2Command
        {
            get
            {
                return _image2Command ?? (_image2Command = new MvxCommand(() =>
                {
                    Image2Action?.Invoke();
                }));
            }
        }

        public Action Image2Action { get; set; }
    }
}