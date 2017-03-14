using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Fabric;
using NetStandardLibrary;

namespace AspNetCoreService.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        IMyInterface myField;
        ServiceContext cxt;

        public ValuesController(IMyInterface arg, StatelessServiceContext context)
        {
            myField = arg;
            cxt = context;
        }

        // GET api/values/MyMethod
        [HttpGet("MyMethod")]
        public IActionResult GetMyMethodValue()
        {
            ServiceEventSource.Current.ServiceMessage(cxt, "Retrieving MyMethod's value");
            return Ok(myField.MyMethod());
        }

        // GET api/values/MyProperty
        [HttpGet("MyProperty")]
        public IActionResult GetMyPropertyValue()
        {
            ServiceEventSource.Current.ServiceMessage(cxt, "Retrieving MyProperty's value");
            return Ok(myField.MyProperty);
        }

        // PUT api/values/MyProperty
        [HttpPut("MyProperty/{value}")]
        public IActionResult SetMyPropertyValue(int value)
        {
            ServiceEventSource.Current.ServiceMessage(cxt, "Setting MyMethod's value");
            myField.MyProperty = value;
            return Ok(value);
        }        
    }
}
