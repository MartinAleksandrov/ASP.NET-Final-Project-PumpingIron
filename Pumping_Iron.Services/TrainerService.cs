namespace Pumping_Iron.Services
{
    using Microsoft.EntityFrameworkCore;
    using Pumping_Iron.Data.Data;
    using Pumping_Iron.Services.Interfaces;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Pumping_Iron.Data.ViewModels.Trainer;

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
    }
}
