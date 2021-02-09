using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Palmary.Loyalty.API
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITestWebService" in both code and config file together.
    [ServiceContract]
    public interface ITestWebService
    {
        [OperationContract]
        string HelloWorld();
    }
}
