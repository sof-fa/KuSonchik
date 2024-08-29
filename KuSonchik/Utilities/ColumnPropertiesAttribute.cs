using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuSonchik.Utilities
{
    class ColumnPropertiesAttribute : Attribute
    {
        public ColumnPropertiesAttribute(string name, double widthInStars)
        {
            Name = name;
            WidthInStars = widthInStars;
        }
        public string Name { get; private set; }
        public double WidthInStars { get; private set; }
    }
}
