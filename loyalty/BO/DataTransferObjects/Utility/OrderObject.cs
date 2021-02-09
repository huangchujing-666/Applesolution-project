using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.Utility
{
    public class OrderObject<T>
    {
        public int id;
        public int display_order;
        public T data_object;
    }
}
