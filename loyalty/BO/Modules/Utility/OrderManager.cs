using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Palmary.Loyalty.BO.DataTransferObjects.Utility;

namespace Palmary.Loyalty.BO.Modules.Utility
{
    public static class OrderManager
    {
        public static List<OrderObject<T>> Reorder<T>(List<OrderObject<T>> objectList, OrderObject<T> update_object)
        {
            // Compatibility for out of range value
            if (update_object.display_order > objectList.Count())
            {
                update_object.display_order = objectList.Count();
            }
            else if (update_object.display_order <= 0)
                update_object.display_order = 1;
            
            // remove target object
            foreach (var o in objectList)
            {
                if (o.id == update_object.id)
                {
                    objectList.Remove(o);
                    break;
                }
            }

            objectList = objectList.OrderBy(x => x.display_order).ToList();
            
            var newList = new List<OrderObject<T>>();

            var target_position = update_object.display_order;

            var listAddCounter = 0;

            for (int i = 0; i < objectList.Count()+1; i++)
            {
                var display_order = i+1;

                if (display_order == target_position)
                {
                    newList.Add(update_object);
                }
                else
                {
                    var theObject = objectList[listAddCounter];
                    theObject.display_order = display_order;
                    newList.Add(theObject);
                    listAddCounter++;
                }
            }

            return newList;
        }
    }
}
