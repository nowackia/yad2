using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Utilities.Common
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class FileNameAttribute : Attribute
    {
        protected string name = string.Empty;

        public FileNameAttribute(string name)
        {
            this.name = name;
        }

        public String Name
        {
            get
            { return this.name; }
        }
    }
}