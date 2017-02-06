using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PocoUtils.Attributes;

namespace PocoUtils
{
    class Person
    {
        [DBField("VisitorFirstName")]
        public String FirstName { get; set; }

        [DBField("VisitorLastName")]
        public String LastName { get; set; }

        [DBField]
        public int Age { get; set; }

        [DBField("Age")]
        public String StringAge { get; set; }


        public override string ToString()
        {
            return FirstName + " " + LastName;
        }
    }
}
