using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM_Dapper
{
    public class Categories
    {
        public Categories()
        {

        }
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public int DepartmentID { get; set; }

    }
}
