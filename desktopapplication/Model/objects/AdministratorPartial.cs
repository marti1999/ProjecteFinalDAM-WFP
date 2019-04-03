using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace desktopapplication.Model
{
    using System;
    using System.Collections.Generic;

    public partial class Administrator
    {
        public override string ToString()
        {
            return this.workerCode + " - " + this.name;
        }
    }
}
