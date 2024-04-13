namespace Pumping_Iron.Services
{
    using Microsoft.EntityFrameworkCore;
    using Pumping_Iron.Data.Data;
    using Pumping_Iron.Services.Interfaces;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Pumping_Iron.Data.ViewModels.Trainer;
    using Pumping_Iron.Data.ViewModels.Client;
    using Pumping_Iron.Data.Models;
    using System.Threading.Channels;

    public class TrainerService : ITrainerService
    {
        private readonly PumpingIronDbContext dbContext;

        public TrainerService(PumpingIronDbContext context)
        {
            dbContext = context;
        }

        //Shows all available trainers with info about them.
        public async Task<IEnumerable<AllTrainersViewModel>> AllTrainersAsync()
        {
            var allTrainers = await dbContext
                .Trainers
                .AsNoTracking()
                .Select(t => new AllTrainersViewModel()
                {
                    Id = t.TrainerId.ToString(),
                    Name = t.Name,
                    Age = t.Age,
                    Gender = t.Gender.ToString(),
                    ImageUrl = t.ImageUrl
                })
                .ToListAsync();

            return allTrainers;
        }

        //Find trainer with the appropriate id if exist 
        public async Task<string> FindTrainerByIdAsync(string id)
        {

            var trainer = await dbContext.Trainers.FirstOrDefaultAsync(t => t.TrainerId.ToString() == id);


            //If there is no such trainer method returns empty string
            if (trainer == null)
            {
                return string.Empty;
            }

            return id;
        }

        //Map trainer entity to ViewModel if such trainer exist
        public async Task<TrainerDetailsViewModel?> GetTrainerDetailsAsync(string id)
        {
            var trinerId = await FindTrainerByIdAsync(id);

            if (string.IsNullOrEmpty(trinerId))
            {
                return null;
            }

            var trainer = await dbContext
                .Trainers
                .Where(t => t.TrainerId.ToString() == trinerId)
                .AsNoTracking()
                .Select(t => new TrainerDetailsViewModel()
                {
                    Id = t.TrainerId.ToString(),
                    Name = t.Name,
                    Information = t.Information,
                    ImageUrl = t.ImageUrl
                })
                .FirstOrDefaultAsync();

            return trainer;
        }

        public async Task<bool> IsClientAlreadyHireTrainerAsync(string clientId)
        {
            var client = await dbContext.Clients.FindAsync(Guid.Parse(clientId));

            //Client does not exist
            if (client == null)
            {
                return false;
            }

            if (client!.Trainer != null)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> HireTrainerAsync(HireTrainerViewModel model, string trainerId, string clientId)
        {
            var client = await dbContext.Clients.FirstOrDefaultAsync(c => c.ClientId.ToString() == clientId);

            // If client already exist
            if (client != null)
            {
                return false;
            }

            // Check if the client has already hired a trainer
            var isHired = await IsClientAlreadyHireTrainerAsync(clientId);

            // Find the trainer associated with the provided trainerId
            var trainer = await dbContext.Trainers.FindAsync(Guid.Parse(trainerId));

            // If the client has not already hired a trainer and a valid trainer is found
            if (!isHired && trainer != null)
            {
                client = new Client()
                {
                    // Assign the trainer to the client
                    ClientId = Guid.Parse(clientId),
                    Name = model.Name,
                    Age = model.Age,
                    ImageUrl = model.ImageUrl,
                    Gender = model.Gender,
                    TrainerId = model.TrainerId,
                    Membership = model.Membership
                };

                // Save changes to the database
                dbContext.Entry(trainer).State = EntityState.Unchanged; // Assuming trainer is not modified
                dbContext.Entry(client).State = EntityState.Added;
                trainer.Clients.Add(client);
                await dbContext.SaveChangesAsync();

            }
            // Return true indicating success
            return true;
        }
    }
}