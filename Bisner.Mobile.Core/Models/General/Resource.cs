namespace Bisner.Mobile.Core.Models.General
{
    public class Resource : IResource
    {
        public string Key { get; set; }
        public string DefaultValue { get; set; }
        public string OverrideValue { get; set; }
        public string MainCategory { get; set; }
        public string SubCategory { get; set; }
    }
}