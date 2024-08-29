using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuSonchik.Utilities
{
    internal class IsNavigationAttribute : Attribute
    {
        public IsNavigationAttribute(bool isNavigation) => IsNavigation = isNavigation;
        public bool IsNavigation { get; }
    }
}
