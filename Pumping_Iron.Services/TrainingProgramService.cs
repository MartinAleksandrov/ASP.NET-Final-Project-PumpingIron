namespace Pumping_Iron.Services
{
    using Microsoft.EntityFrameworkCore;
    using Pumping_Iron.Data.Data;
    using Pumping_Iron.Data.ViewModels.TrainingPrograms;
    using Pumping_Iron.Services.Interfaces;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class TrainingProgramService : ITrainingProgramService
    {
        private readonly PumpingIronDbContext dbContext;

        public TrainingProgramService(PumpingIronDbContext context)
        {
            dbContext = context;
        }

        //Shows all available trainers with info about them.
        public async Task<IEnumerable<AllTrainingProgramsViewModel>> AllTrainingProgramsAsync()
        {
            var allTrainingPrograms = await dbContext
                .TrainingPrograms
                .AsNoTracking()
                .Select(p => new AllTrainingProgramsViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Duration = p.Duration,
                    ImageUrl = p.ImageUrl
                })
                .ToListAsync();

            return allTrainingPrograms;
        }
    }
}