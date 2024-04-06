namespace Pumping_Iron.Services
{
    using Microsoft.EntityFrameworkCore;
    using Pumping_Iron.Data.Data;
    using Pumping_Iron.Data.Models;
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

        //Create new Training Program
        public async Task<bool> CreateProgramAsync(CreateProgramViewModel model, int id,string trainerId)
        {
            // Check if the program already exists with the given id or name
            if (await ExistByIdAsync(id) || dbContext.TrainingPrograms.Any(p => p.Name == model.Name))
            {
                return false; // Program already exists
            }

            // Check if the trainer exists
            var trainer = await dbContext.Trainers.FindAsync(trainerId);
            if (trainer == null)
            {
                return false; // Trainer not found
            }

            // Create a new training program
            var newProgram = new TrainingProgram
            {
                Name = model.Name,
                Description = model.Description,
                Duration = model.Duration,
                ImageUrl = model.ImageUrl,
                Trainer = trainer
            };

            dbContext.TrainingPrograms.Add(newProgram);
            await dbContext.SaveChangesAsync();

            return true; // Program created successfully
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