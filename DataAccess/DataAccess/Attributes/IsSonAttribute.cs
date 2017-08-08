using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataAccess.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IsSonAttribute : Attribute
    {
        public bool Value { get; set; }

        public IsSonAttribute() { }

        public IsSonAttribute(bool value)
        {
            this.Value = value;
        }
    }
}
