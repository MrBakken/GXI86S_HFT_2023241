using GXI86S_HFT_2023241.Logic.InterfaceLogic;
using GXI86S_HFT_2023241.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace GXI86S_HFT_2023241.Endpoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        ICustomerLogic logic;

        public CustomerController(ICustomerLogic logic)
        {
            this.logic = logic;
        }


        // GET: api/<CustomerController>
        [HttpGet]
        public IEnumerable<Customer> ReadAll()
        {
            return this.logic.ReadAll();
        }

        // GET api/<CustomerController>/5
        [HttpGet("{id}")]
        public Customer Read(int id)
        {
            return this.logic.Read(id);
        }

        // POST api/<CustomerController>
        [HttpPost]
        public void Create([FromBody] Customer value)
        {
            this.logic.Create(value);
        }

        // PUT api/<CustomerController>/5
        [HttpPut]
        public void Update([FromBody] Customer value)
        {
            this.logic.Update(value);
        }

        // DELETE api/<CustomerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            this.logic.Delete(id);
        }
    }
}
