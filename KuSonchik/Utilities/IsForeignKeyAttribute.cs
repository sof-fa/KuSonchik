using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KuSonchik.Utilities
{
    internal class IsForeignKeyAttribute : Attribute
    {
        public IsForeignKeyAttribute(bool isForeignKey, Type typeForeignKey, string nameProperty)
        {
            IsForeignKey = isForeignKey;
            TypeForeignKey = typeForeignKey;
            PropertyInfo = typeForeignKey.GetProperty(nameProperty);
        }
        public bool IsForeignKey { get; }
        public Type TypeForeignKey { get; }
        public PropertyInfo PropertyInfo { get; }
    }
}
