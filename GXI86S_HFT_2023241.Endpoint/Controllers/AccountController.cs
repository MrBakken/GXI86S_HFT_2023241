using GXI86S_HFT_2023241.Endpoint.Services;
using GXI86S_HFT_2023241.Logic.InterfaceLogic;
using GXI86S_HFT_2023241.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace GXI86S_HFT_2023241.Endpoint.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        IAccountLogic logic;
        private readonly IHubContext<SignalRHub> hub;

        public AccountController(IAccountLogic logic, IHubContext<SignalRHub> hub)
        {
            this.logic = logic;
            this.hub = hub;
        }

        [HttpGet]
        public IEnumerable<Account> ReadAll()
        {
            return this.logic.ReadAll();
        }

        [HttpGet("{id}")]
        public Account Read(int id)
        {
            return this.logic.Read(id);
        }

        [HttpPost]
        public void Create([FromBody] Account value)
        {
            this.logic.Create(value);
            this.hub.Clients.All.SendAsync("AccountCreated", value);
        }

        [HttpPut]
        public void Update([FromBody] Account value)
        {
            this.logic.Update(value);
            this.hub.Clients.All.SendAsync("AccountUpdated", value);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var accountToDelete = this.logic.Read(id);
            this.logic.Delete(id);
            this.hub.Clients.All.SendAsync("AccountDeleted", accountToDelete);
        }
    }
}
