using System.Collections.Generic;

namespace Bisner.Mobile.Core.ViewModels.Workspace.Models
{
    /// <summary>
    /// Section with Image and text
    /// </summary>
    public class WorkspaceMenuSection : WorkspaceMenuSectionBase
    {
        public WorkspaceMenuSection()
        {

        }

        public WorkspaceMenuSection(IEnumerable<WorkspaceMenuItem> items)
            : base(items)
        {
        }

        public override WorkspaceMenuSectionType Type
        {
            get { return WorkspaceMenuSectionType.Normal; }
        }
    }
}
