using System;

namespace Bisner.Mobile.Core.Models.Base
{
    public interface IDisplayUser
    {
        Guid UserId { get; set; }
        string AvatarUrl { get; set; }
        string DisplayName { get; set; }
    }
}