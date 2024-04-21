namespace Pumping_Iron.Services
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualStudio.Web.CodeGeneration.Design;
    using Pumping_Iron.Data.Data;
    using Pumping_Iron.Data.Models;
    using Pumping_Iron.Data.ViewModels.Client;
    using Pumping_Iron.Data.ViewModels.Diet;
    using Pumping_Iron.Data.ViewModels.Trainer;
    using Pumping_Iron.Services.Interfaces;
    using System.Collections.Generic;
    using System.Threading.Tasks;

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

        ////Map trainer entity to ViewModel if such trainer exist
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
                    Trainer = trainer,
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

        public async Task<bool> RemoveClient(string clientId, string trainerId)
        {
            var trainer = await dbContext.Trainers.FirstOrDefaultAsync(t => t.TrainerId.ToString() == trainerId);

            if (trainer == null)
            {
                return false;
            }

            var client = trainer.Clients.FirstOrDefault(c => c.ClientId.ToString() == clientId);

            if (client == null)
            {
                return false;
            }

            trainer.Clients.Remove(client);
            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<AddProgramViewModel>?> GetAllTrainerPrograms(string trainerId,string clientId)
        {
            var isExist = await FindTrainerByIdAsync(trainerId);

            if (string.IsNullOrEmpty(isExist))
            {
                return null;
            }

            var trainerPrograms = await dbContext.Trainers
                .Where(t => t.TrainerId.ToString() == trainerId)
                .Include(t => t.TrainingPrograms)
                .SelectMany(t => t.TrainingPrograms.Select(tp => new AddProgramViewModel
                {
                    Id = tp.Id,
                    Name = tp.Name,
                    Description = tp.Description,
                    Duration = tp.Duration,
                    ImageUrl = tp.ImageUrl,
                    ClientId = clientId
                }))
                .ToListAsync();

            return trainerPrograms;
        }

        public async Task<bool> AddProgramToClient(int programId, string clientId)
        {
            var client = await dbContext.Clients.FirstOrDefaultAsync(c => c.ClientId.ToString() == clientId);
            var program = await dbContext.TrainingPrograms.FirstOrDefaultAsync(t => t.Id == programId);

            if (client == null || program == null)
            {
                return false;
            }

            var result = await IsClientAlreadyHaveProgramAsync(clientId);

            if (result)
            {
                return false;
            }

            // Assign the training program to the client's navigation property
            client.TrainingProgram = program;
            program.Clients.Add(client);

            // Save changes to the database
            await dbContext.SaveChangesAsync();

            return true;
        }


        public async Task<IEnumerable<AddDietViewModel>?> GetAllTrainerDiets(string trainerId, string clientId)
        {
            var isExist = await FindTrainerByIdAsync(trainerId);

            if (string.IsNullOrEmpty(isExist))
            {
                return null;
            }

            var trainerDiets = await dbContext.Trainers
                .Where(t => t.TrainerId.ToString() == trainerId)
                .Include(t => t.Diets)
                .SelectMany(t => t.Diets.Select(tp => new AddDietViewModel
                {
                    Id = tp.Id,
                    Name = tp.Name,
                    Description = tp.Description,
                    ImageUrl = tp.ImageUrl,
                    ClientId = clientId
                }))
                .ToListAsync();

            return trainerDiets;
        }

        public async Task<bool> AddDietToClient(int dietId, string clientId)
        {
            var client = await dbContext.Clients.FirstOrDefaultAsync(c => c.ClientId.ToString() == clientId);
            var diet = await dbContext.Diets.FirstOrDefaultAsync(t => t.Id == dietId);

            if (client == null || diet == null)
            {
                return false;
            }

            var result = await IsClientAlreadyHaveDietAsync(clientId);

            if (result)
            {
                return false;
            }

            // Assign the diet to the client's navigation property
            client.Diet = diet;
            diet.Clients.Add(client);
            // Save changes to the database
            await dbContext.SaveChangesAsync();

            return true;
        }

        private async Task<bool> IsClientAlreadyHaveProgramAsync(string clientId)
        {
            var client = await dbContext.Clients.FindAsync(Guid.Parse(clientId));

            if (client!.TrainingProgram != null)
            {
                return true;
            }

            return false;
        }

        private async Task<bool> IsClientAlreadyHaveDietAsync(string clientId)
        {
            var client = await dbContext.Clients.FindAsync(Guid.Parse(clientId));

            if (client!.Diet != null)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> RemoveProgramFromClientAsync(int programId, string clientId)
        {
            var client = await dbContext.Clients.FirstOrDefaultAsync(c => c.ClientId.ToString() == clientId);
            var trainingProgram = await dbContext.TrainingPrograms.FirstOrDefaultAsync(tp => tp.Id == programId);

            if (client != null && client.TrainingProgramId == programId && trainingProgram != null)
            {
                client.TrainingProgram = null;
                trainingProgram.Clients.Remove(client);
                await dbContext.SaveChangesAsync(); 
                return true;
            }

            return false;
        }

        public async Task<bool> RemoveDietFromClientAsync(int dietId, string clientId)
        {
            var client = await dbContext.Clients.FirstOrDefaultAsync(c => c.ClientId.ToString() == clientId);
            var diet = await dbContext.Diets.FirstOrDefaultAsync(d => d.Id == dietId);

            if (client != null && client.DietId == dietId && diet != null)
            {
                client.Diet = null;
                diet.Clients.Remove(client);
                await dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}