using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetStandardLibrary;

namespace AspNetCoreService.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        IMyInterface myField;

        public ValuesController(IMyInterface arg)
        {
            myField = arg;
        }

        // GET api/values/MyMethod
        [HttpGet("MyMethod")]
        public IActionResult GetMyMethodValue()
        {
            ServiceEventSource.Current.Message("Retrieving MyMethod's value");
            return Ok(myField.MyMethod());
        }

        // GET api/values/MyProperty
        [HttpGet("MyProperty")]
        public IActionResult GetMyPropertyValue()
        {
            ServiceEventSource.Current.Message("Retrieving MyProperty's value");
            return Ok(myField.MyProperty);
        }

        // PUT api/values/MyProperty
        [HttpPut("MyProperty/{value}")]
        public IActionResult SetMyPropertyValue(int value)
        {
            ServiceEventSource.Current.Message("Setting MyMethod's value");
            myField.MyProperty = value;
            return Ok(value);
        }        
    }
}
