using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Palmary.Loyalty.API
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TestWebService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select TestWebService.svc or TestWebService.svc.cs at the Solution Explorer and start debugging.
    public class TestWebService : ITestWebService
    {
        public string HelloWorld()
        {
            return "Hello World";
        }
    }
}
