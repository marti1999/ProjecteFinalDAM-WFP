using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WSRobaSegonaMa.Models
{
    partial class  Requestor
    {
        public override string ToString()
        {
            return this.name + " " + this.lastName + ", " + this.dni;

        }
    }
}
