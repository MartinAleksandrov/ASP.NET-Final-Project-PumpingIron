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
    }
}
