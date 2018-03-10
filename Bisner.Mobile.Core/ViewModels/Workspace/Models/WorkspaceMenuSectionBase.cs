using System.Collections.Generic;
using Bisner.Mobile.Core.ViewModels.Base.Models;

namespace Bisner.Mobile.Core.ViewModels.Workspace.Models
{
    /// <summary>
    /// Workspace menu section base class
    /// </summary>
    public abstract class WorkspaceMenuSectionBase : SectionBase<WorkspaceMenuItem>
    {
        public abstract WorkspaceMenuSectionType Type { get; }

        protected WorkspaceMenuSectionBase()
        {

        }

        protected WorkspaceMenuSectionBase(IEnumerable<WorkspaceMenuItem> items)
            : base(items)
        {
        }
    }
}