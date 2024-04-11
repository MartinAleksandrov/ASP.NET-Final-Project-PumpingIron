namespace Pumping_Iron.Core.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Pumping_Iron.Infrastructure.Extensions;
    using Pumping_Iron.Services.Interfaces;

    public class ClientController : Controller
    {
        private readonly IClientService clientService;

        public ClientController(IClientService service)
        {
            clientService = service;
        }

        [HttpGet]
        [Authorize(Roles = "Coach")]
        public async Task<IActionResult> TrainerClients()
        {
            var userId = User.GetId();

            // Retrieve clients associated with the current trainer
            var clients = await clientService.GetMyClientsAsync(userId);

            // Check if clients were successfully retrieved
            if (clients == null)
            {
                return BadRequest("Trainer does not exist or error occurred.");
            }

            // If clients were successfully retrieved, return a View with the clients data
            return View(clients);
        }
    }
}