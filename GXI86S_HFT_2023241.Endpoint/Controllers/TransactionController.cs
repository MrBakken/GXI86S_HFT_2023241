using GXI86S_HFT_2023241.Endpoint.Services;
using GXI86S_HFT_2023241.Logic.InterfaceLogic;
using GXI86S_HFT_2023241.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GXI86S_HFT_2023241.Endpoint.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        ITransactionLogic logic;
        private readonly IHubContext<SignalRHub> hub;

        public TransactionController(ITransactionLogic logic, IHubContext<SignalRHub> hub)
        {
            this.logic = logic;
            this.hub = hub;
        }

        [HttpGet]
        public IEnumerable<Transaction> ReadAll()
        {
            return this.logic.ReadAll();
        }

        [HttpGet("{id}")]
        public Transaction Read(int id)
        {
            return this.logic.Read(id);
        }

        [HttpPost]
        public void Create([FromBody] Transaction value)
        {
            this.logic.Create(value);
            this.hub.Clients.All.SendAsync("TransactionCreated", value);
        }

        [HttpPut]
        public void Update([FromBody] Transaction value)
        {
            this.logic.Update(value);
            this.hub.Clients.All.SendAsync("TransactionUpdated", value);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var transactionToDelete = this.logic.Read(id);
            this.logic.Delete(id);
            this.hub.Clients.All.SendAsync("TransactionDeleted", transactionToDelete);
        }
    }
}
