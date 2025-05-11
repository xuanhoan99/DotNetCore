using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCore.Application.Modules.Common
{
    public static class HCorePermissions
    {
        public class Prefix
        {
            public const string Administration = "Pages.Administration";
            public const string Main = "Pages.Main";
        }
        public class Page
        {
            public const string SysMenu = "SysMenu";
        }
        public class Action
        {
            public const string Create = "Create";
            public const string Update = "Update";
            public const string Delete = "Delete";
            public const string View = "View";
            public const string Approve = "Approve";
            public const string Search = "Search";
            public const string Reject = "Reject";
        }
    }
}
