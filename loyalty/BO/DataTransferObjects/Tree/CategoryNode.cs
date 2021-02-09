using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.Tree
{
    public class CategoryNode
    {
        public int id { get; set; }

        public int ParentID { get; set; }

        public string text { get; set; }

        public bool expanded { get; set; }

        public List<CategoryNode> children { get; set; }
    }
}
