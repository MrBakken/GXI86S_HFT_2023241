using GXI86S_HFT_2023241.Logic;
using GXI86S_HFT_2023241.Logic.InterfaceLogic;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GXI86S_HFT_2023241.Endpoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NonCrud : ControllerBase
    {
        ICustomerLogic logic;
        public NonCrud(ICustomerLogic logic)
        {
            this.logic = logic;
        }

        // GET: api/<NonCrud>
        [HttpGet]
        public IEnumerable<CustomerLogic.> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
