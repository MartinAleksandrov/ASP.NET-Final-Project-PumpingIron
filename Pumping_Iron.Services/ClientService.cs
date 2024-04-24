namespace Pumping_Iron.Services
{
    using Microsoft.EntityFrameworkCore;
    using Pumping_Iron.Data.Data;
    using Pumping_Iron.Data.Models;
    using Pumping_Iron.Data.ViewModels.Client;
    using Pumping_Iron.Data.ViewModels.Trainer;
    using Pumping_Iron.Services.Interfaces;
    using System.Threading.Tasks;

    public class ClientService : IClientService
    {
        private readonly PumpingIronDbContext dbContext;
        private readonly ITrainerService trainerService;

        public ClientService(PumpingIronDbContext context, ITrainerService service)
        {
            dbContext = context;
            trainerService = service;
        }

        /// <summary>
        /// Retrieves the clients associated with a trainer identified by the provided trainerId.
        /// </summary>
        /// <param name="trainerId">The ID of the trainer.</param>
        /// <returns>
        /// A collection of MyClientsViewModel representing the clients associated with the trainer.
        /// Returns null if the trainer does not exist.
        /// </returns>
        public async Task<PaginatedList<MyClientsViewModel>?> GetMyClientsAsync(string trainerId, int pageNumber, int pageSize)
        {
            // Check if the trainer exists
            var isTrainerExist = await trainerService.FindTrainerByIdAsync(trainerId);

            // If the trainer does not exist, return null
            if (string.IsNullOrEmpty(isTrainerExist))
            {
                return null;
            }

            // Retrieve the total count of clients associated with the trainer
            var totalCount = await dbContext.Clients
                    .Where(c => c.TrainerId.ToString() == trainerId)
                    .CountAsync();

            // Retrieve the clients associated with the trainer with pagination
            var myClients = await dbContext.Clients
                    .Where(c => c.TrainerId.ToString() == trainerId)
                    .Include(c => c.Membership)
                    .Include(c => c.TrainingProgram)
                    .Select(c => new MyClientsViewModel
                    {
                        // Map client properties to MyClientsViewModel properties
                        ClientId = c.ClientId,
                        Name = c.Name,
                        Age = c.Age,
                        Gender = c.Gender.ToString(),
                        ImageUrl = c.ImageUrl,
                        MembershipName = c.Membership!.TypeMembership.ToString(),
                        TrainingProgram = c.TrainingProgram != null ? new ClientProgramViewModel
                        {
                            // Map training program properties to ClientProgramViewModel properties
                            Id = c.TrainingProgram.Id,
                            Name = c.TrainingProgram.Name,
                            Description = c.TrainingProgram.Description,
                            Duration = c.TrainingProgram.Duration,
                            ImageUrl = c.TrainingProgram.ImageUrl
                        } : null,// Return null if TrainingProgram is null
                        Diet = c.Diet != null ? new ClientDietViewModel
                        {
                            // Map training program properties to ClientProgramViewModel properties
                            Id = c.Diet.Id,
                            Name = c.Diet.Name,
                            Description = c.Diet.Description,
                            ImageUrl = c.Diet.ImageUrl
                        } : null// Return null if Diet is null
                    })
                    .OrderBy(c => c.Name) // You can order by any property you want
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

            // Create a PaginatedList instance from the retrieved clients
            var paginatedClients = new PaginatedList<MyClientsViewModel>(myClients, totalCount, pageNumber, pageSize);

            // Return the paginated list of clients associated with the trainer
            return paginatedClients;
        }

        //Retrieve client diet info
        public async Task<ClientDietViewModel?> GetMyDietInfo(string userId)
        {
            // Check if the user (client) exists
            var user = await IsClientExist(userId);

            // If the user does not exist or is not associated with a diet, return null
            if (user == null || user.Diet == null)
            {
                return null;
            }

            // Create a view model to represent the diet details
            var dietViewModel = new ClientDietViewModel()
            {
                // Assign properties from the user's diet
                Id = user.Diet.Id,
                Name = user.Diet.Name,
                Description = user.Diet.Description,
                ImageUrl = user.Diet.ImageUrl
            };

            // Return the populated diet view model
            return dietViewModel;
        }

        //Retrieve client training program info
        public async Task<ClientProgramViewModel?> GetMyProgramInfo(string userId)
        {
            // Check if the user (client) exists
            var user = await IsClientExist(userId);

            // If the user does not exist or is not associated with a training program, return null
            if (user == null || user.TrainingProgramId == null)
            {
                return null;
            }

            // Retrieve the training program from the database based on the TrainingProgramId associated with the user
            var program = await dbContext.TrainingPrograms.FirstOrDefaultAsync(tp => tp.Id == user.TrainingProgramId);

            // Create a view model to represent the training program details
            var programViewModel = new ClientProgramViewModel()
            {
                // Assign properties from the retrieved training program
                Id = program!.Id, // Assuming Id is a string or an integer
                Name = program.Name,
                Description = program.Description,
                ImageUrl = program.ImageUrl,
                Duration = program.Duration
            };

            // Return the populated training program view model
            return programViewModel;
        }

        //Retrieve all trainer info
        public async Task<TrainerDetailsViewModel?> GetMyTrainerInfo(string userId)
        {
            // Check if the user (client) exists
            var user = await IsClientExist(userId);

            // If the user does not exist or is not associated with a trainer, return null
            if (user == null || user.TrainerId == null)
            {
                return null;
            }

            // Retrieve the trainer from the database based on the TrainerId associated with the user
            var trainer = await dbContext.Trainers.FirstOrDefaultAsync(t => t.TrainerId == user.TrainerId);

            // Create a view model to represent the trainer details
            var trainerViewModel = new TrainerDetailsViewModel()
            {
                // Assign properties from the retrieved trainer
                Id = trainer!.TrainerId.ToString()!, // Using ToString() to convert the Guid to a string
                Name = trainer.Name,
                ImageUrl = trainer.ImageUrl,
                Information = trainer.Information
            };

            // Return the populated trainer view model
            return trainerViewModel;
        }

        //Remove trainer from client
        public async Task<bool> RemoveMyTrainer(string clientId)
        {
            // Check if the client exists
            var client = await IsClientExist(clientId);

            // If the client exists and is associated with a trainer
            if (client != null && client.TrainerId != null)
            {
                // Remove the client from the database
                dbContext.Clients.Remove(client);

                // Save the changes to the database
                await dbContext.SaveChangesAsync();

                // Remove the client from the trainer's clients list
                var isClientRemovedFromTrainerClients = await trainerService.RemoveClient(clientId, client.TrainerId.ToString()!);

                // Return true to indicate successful removal
                return true;
            }

            // Return false if the client doesn't exist or is not associated with a trainer
            return false;
        }

        //Check if client exist
        private async Task<Client?> IsClientExist(string userId)
        {
            var client = await dbContext.Clients.FirstOrDefaultAsync(c => c.ClientId.ToString() == userId);

            if (client == null)
            {
                return null;
            }

            return client;
        }
    }
}