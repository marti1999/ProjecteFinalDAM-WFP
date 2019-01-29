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
        public int Id { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string lastName { get; set; }
        public string password { get; set; }
        public System.DateTime dateCreated { get; set; }
        public bool isSuper { get; set; }
        public bool active { get; set; }
        public string workerCode { get; set; }
        public int Language_Id { get; set; }
        public int Warehouse_Id { get; set; }

        public virtual Language Language { get; set; }
        public virtual Warehouse Warehouse { get; set; }
    }
}
