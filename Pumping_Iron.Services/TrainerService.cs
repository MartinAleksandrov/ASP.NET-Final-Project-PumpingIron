namespace Pumping_Iron.Services
{
    using Microsoft.EntityFrameworkCore;
    using Pumping_Iron.Core.ViewModels;
    using Pumping_Iron.Data.Data;
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
                .Select(t => new AllTrainersViewModel()
                {
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
