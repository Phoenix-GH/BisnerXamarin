namespace Bisner.Mobile.Core.Models.General
{
    public interface IResource
    {
        string Key { get; set; }

        string DefaultValue { get; set; }

        string OverrideValue { get; set; }

        string MainCategory { get; set; }

        string SubCategory { get; set; }
    }
}