using MvvmCross.Plugins.PictureChooser;

namespace Bisner.Mobile.Core.Helpers
{
    public interface IMvxBisnerImageTask : IMvxPictureChooserTask
    {
        void ChangeDevice(bool front);
    }
}