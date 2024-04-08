namespace Pumping_Iron.Services
{
    using Microsoft.EntityFrameworkCore;
    using Pumping_Iron.Data.Data;
    using Pumping_Iron.Data.Models;
    using Pumping_Iron.Data.ViewModels.Diet;
    using Pumping_Iron.Services.Interfaces;

    public class DietService : IDietService
    {
        private readonly PumpingIronDbContext dbContext;

        public DietService(PumpingIronDbContext context)
        {
            dbContext = context;
        }

        //Shows all available diets with info about them.
        public async Task<IEnumerable<AllDietsViewModel>> AllDietsAsync()
        {
            var allDiets = await dbContext.Diets
                .AsNoTracking()
                .Select(d => new AllDietsViewModel()
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    ImageUrl = d.ImageUrl,
                })
                .ToListAsync();

            return allDiets;
        }

        public async Task<bool> CreateDietAsync(CreateDietViewModel model, string trainerId)
        {
            // Check if the diet already exists with the given id or name
            if (await ExistByIdAsync(model.Id) || dbContext.Diets.Any(p => p.Name == model.Name))
            {
                return false; // Program already exists
            }

            // Check if the trainer exists
            var trainer = await dbContext.Trainers.FirstOrDefaultAsync(t => t.TrainerId.ToString() == trainerId);
            if (trainer == null)
            {
                return false; // Trainer not found
            }

            // Create a new diet
            var newDiet = new Diet
            {
                Name = model.Name,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                Trainer = trainer
            };

            dbContext.Diets.Add(newDiet);
            trainer.Diets.Add(newDiet);
            await dbContext.SaveChangesAsync();

            return true; // Diet created successfully
        }

        //Check if there is such Diet by Id
        public async Task<bool> ExistByIdAsync(int id)
        {
            var diet = await dbContext.Diets.FirstOrDefaultAsync(d => d.Id == id);

            if (diet == null)
            {
                return false;
            }

            return true;
        }

        //Map trainer entity to ViewModel if such Diet exist
        public async Task<AllDietsViewModel?> GetDetaisByIdAsync(int id)
        {
            var result = await ExistByIdAsync(id);

            //If there is no such diet method returns null
            if (!result)
            {
                return null;
            }


            var model = await dbContext.Diets.Where(d => d.Id == id)
                   .AsNoTracking()
                   .Select(d => new AllDietsViewModel()
                   {
                       Id = d.Id,
                       Name = d.Name,
                       Description = d.Description,
                       ImageUrl = d.ImageUrl
                   })
                   .FirstOrDefaultAsync();

            return model!;
        }
    }
}