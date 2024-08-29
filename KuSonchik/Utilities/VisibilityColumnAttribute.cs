using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KuSonchik.Utilities
{
    class VisibilityColumnAttribute : Attribute
    {
        public VisibilityColumnAttribute(Visibility visibility, Visibility id_visibility)
        {
            Visibility = visibility;
            ID_Visibility = id_visibility;
        }
        public Visibility Visibility { get; }
        public Visibility ID_Visibility { get; }
    }
}
