using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrugsInfo
{
    public class Drug
    {
        public int DrugID { get; set; }
        public string DrugName { get; set; }
        public string GenericGroup { get; set; }
        public string ShelfNo { get; set; }
        public double UnitPrice { get; set; }
    }
}
