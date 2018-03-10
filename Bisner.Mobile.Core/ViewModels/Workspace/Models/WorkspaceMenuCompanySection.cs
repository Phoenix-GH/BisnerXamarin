using System.Collections.Generic;

namespace Bisner.Mobile.Core.ViewModels.Workspace.Models
{
    /// <summary>
    /// Section with company name and add button
    /// </summary>
    public class WorkspaceMenuCompanySection : WorkspaceMenuSectionBase
    {
        public WorkspaceMenuCompanySection()
        {

        }

        public WorkspaceMenuCompanySection(IEnumerable<WorkspaceMenuItem> items)
            : base(items)
        {
        }

        public override WorkspaceMenuSectionType Type
        {
            get { return WorkspaceMenuSectionType.Company; }
        }
    }
}