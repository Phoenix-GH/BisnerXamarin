using System.Collections.Generic;

namespace Bisner.Mobile.Core.ViewModels.Workspace.Models
{
    /// <summary>
    /// Section with company name and add button
    /// </summary>
    public class WorkspaceMenuShowMoreSection : WorkspaceMenuSectionBase
    {
        public WorkspaceMenuShowMoreSection()
        {

        }

        public WorkspaceMenuShowMoreSection(IEnumerable<WorkspaceMenuItem> items)
            : base(items)
        {
        }

        public override WorkspaceMenuSectionType Type
        {
            get { return WorkspaceMenuSectionType.ShowMore; }
        }
    }
}