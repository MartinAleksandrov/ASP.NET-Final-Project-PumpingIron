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
        public async Task<IEnumerable<MyClientsViewModel>?> GetMyClientsAsync(string trainerId)
        {
            // Check if the trainer exists
            var isTrainerExist = await trainerService.FindTrainerByIdAsync(trainerId);

            // If the trainer does not exist, return null
            if (string.IsNullOrEmpty(isTrainerExist))
            {
                return null;
            }

            // Retrieve the clients associated with the trainer
            var myClients = await dbContext.Clients
                .AsNoTracking()
                .Where(c => c.TrainerId.ToString() == trainerId)
                .Include(c => c.Membership)
                .Include(c => c.TrainingProgram)
                .Select(c => new MyClientsViewModel
                {
                    // Map client properties to MyClientsViewModel properties
                    Name = c.Name,
                    Age = c.Age,
                    Gender = c.Gender.ToString(),
                    ImageUrl = c.ImageUrl,
                    MembershipName = c.Membership!.TypeMembership.ToString()

                    //TrainingProgram = new ClientProgramViewModel
                    //{
                    //    // Map training program properties to ClientProgramViewModel properties
                    //    Id = c.TrainingProgram.Id,
                    //    Name = c.TrainingProgram.Name,
                    //    Description = c.TrainingProgram.Description,
                    //    Duration = c.TrainingProgram.Duration,
                    //    ImageUrl = c.TrainingProgram.ImageUrl
                    //}
                })
                .ToListAsync();

            // Return the collection of clients associated with the trainer
            return myClients;
        }

        public async Task<ClientDietViewModel?> GetMyDietInfo(string userId)
        {
            var user = await IsClientExist(userId);

            if (user == null || user.Diet == null || user.DietId != 0)
            {
                return null;
            }

            var dietViewModel = new ClientDietViewModel()
            {
                Id = user.Diet.Id,
                Name = user.Diet.Name,
                Description = user.Diet.Description,
                ImageUrl = user.Diet.ImageUrl
            };

            return dietViewModel;
        }

        public async Task<ClientProgramViewModel?> GetMyProgramInfo(string userId)
        {
            var user = await IsClientExist(userId);

            if (user == null || user.TrainingProgram == null || user.TrainingProgramId != 0)
            {
                return null;
            }

            var programViewModel = new ClientProgramViewModel()
            {
                Id = user.TrainingProgram.Id,
                Name = user.TrainingProgram.Name,
                Description = user.TrainingProgram.Description,
                ImageUrl = user.TrainingProgram.ImageUrl,
                Duration = user.TrainingProgram.Duration
            };

            return programViewModel;
        }

        public async Task<TrainerDetailsViewModel?> GetMyTrainerInfo(string userId)
        {
            var user = await IsClientExist(userId);

            if (user == null || user.TrainerId == null)
            {
                return null;
            }

            var trainer = await dbContext.Trainers.FirstOrDefaultAsync(t => t.TrainerId == user.TrainerId);

            var trainerViewModel = new TrainerDetailsViewModel()
            {
                Id = trainer!.TrainerId.ToString()!,
                Name = trainer.Name,
                ImageUrl = trainer.ImageUrl,
                Information = trainer.Information

            };

            return trainerViewModel;
        }

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