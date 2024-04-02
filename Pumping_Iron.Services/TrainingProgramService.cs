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

        //Check if the program exist by id 
        public async Task<bool> ExistByIdAsync(int id)
        {
            var exist = await dbContext.TrainingPrograms.AnyAsync(p => p.Id == id);

            if (!exist)
            {
                return false;
            }

            return true;
        }

        //Map trainer entity to ViewModel if such trainingProgram exist
        public async Task<AllTrainingProgramsViewModel?> GetProgramDetailsAsync(int id)
        {
            var result = await ExistByIdAsync(id);

            if (!result)
            {
                return null;
            }

            var model = await dbContext
                .TrainingPrograms
                .Where(p => p.Id == id)
                .AsNoTracking()
                .Select(p => new AllTrainingProgramsViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Duration = p.Duration,
                    ImageUrl = p.ImageUrl,
                }).FirstOrDefaultAsync();

            return model;
        }
    }
}