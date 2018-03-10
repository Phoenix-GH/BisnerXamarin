using System.Collections.Generic;

namespace Bisner.Mobile.Core.ViewModels.Workspace.Models
{
    /// <summary>
    /// Section with Image and text
    /// </summary>
    public class WorkspaceMenuImageSection : WorkspaceMenuSectionBase
    {
        public WorkspaceMenuImageSection()
        {

        }

        public WorkspaceMenuImageSection(IEnumerable<WorkspaceMenuItem> items)
            : base(items)
        {
        }

        public override WorkspaceMenuSectionType Type
        {
            get { return WorkspaceMenuSectionType.Image; }
        }
    }
}