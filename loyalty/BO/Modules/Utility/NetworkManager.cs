using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.Modules.Utility
{
    public static class NetworkManager
    {
        public static string GetClientIP()
        {
            string ip = "";
            var props = OperationContext.Current.IncomingMessageProperties;
            var endpointProperty = props[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            if (endpointProperty != null)
            {
                ip = endpointProperty.Address;
            }

            return ip; 
        }
    }
}
