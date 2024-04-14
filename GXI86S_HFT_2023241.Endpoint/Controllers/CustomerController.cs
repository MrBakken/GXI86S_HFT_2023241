using GXI86S_HFT_2023241.Endpoint.Services;
using GXI86S_HFT_2023241.Logic.InterfaceLogic;
using GXI86S_HFT_2023241.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;

namespace GXI86S_HFT_2023241.Endpoint.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        ICustomerLogic logic;
        private readonly IHubContext<SignalRHub> hub;

        public CustomerController(ICustomerLogic logic, IHubContext<SignalRHub> hub)
        {
            this.logic = logic;

            this.hub = hub;
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
            this.hub.Clients.All.SendAsync("CustomerCreated", value);
        }

        // PUT api/<CustomerController>/5
        [HttpPut]
        public void Update([FromBody] Customer value)
        {
            this.logic.Update(value);
            this.hub.Clients.All.SendAsync("CustomerUpdated", value);
        }

        // DELETE api/<CustomerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var custumerToDelete = this.logic.Read(id);
            this.logic.Delete(id);
            this.hub.Clients.All.SendAsync("CustomerDeleted", custumerToDelete);
        }
    }
}
