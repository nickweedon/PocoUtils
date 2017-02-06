using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace PocoUtils.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    // ReSharper disable once InconsistentNaming
    public class DBFieldAttribute : Attribute
    {
        public DBFieldAttribute(string fieldName = null)
        {
            FieldName = fieldName;
        }

        public string FieldName { get; set; }
    }
}
