using System;

namespace Bisner.ApiModels.Integrations
{
    public class ApiAccessControlModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
    }

    public enum LockState
    {
        Close = 0,
        Opening = 1,
        Open = 2,
    }
}