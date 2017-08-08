using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataAccess.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IsFatherAttribute : Attribute
    {
        public bool Value { get; set; }

        public IsFatherAttribute() { }

        public IsFatherAttribute(bool value)
        {
            this.Value = value;
        }
    }
}