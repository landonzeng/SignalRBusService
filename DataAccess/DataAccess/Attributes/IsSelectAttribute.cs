using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataAccess.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IsSelectAttribute : Attribute
    {
        public bool Value { get; set; }
        //public string Name { get; set; }

        public IsSelectAttribute() { }

        public IsSelectAttribute(bool value)
        {
            this.Value = value;
            //this.Name = name;
        }
    }
}
