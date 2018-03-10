using System;

namespace Bisner.ApiModels.Security.Roles
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class BisnerRolesAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class BisnerDefaultRoleAttribute : Attribute
    {
        public bool Admin { get; set; }

        public bool User { get; set; }

        public bool Guest { get; set; }

        public string RequiresParent { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public bool Enabled { get; set; } = true;
    }
}