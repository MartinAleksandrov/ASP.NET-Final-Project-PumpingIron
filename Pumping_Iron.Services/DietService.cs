namespace Pumping_Iron.Services
{
    using Microsoft.EntityFrameworkCore;
    using Pumping_Iron.Data.Data;
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
        public async Task<IEnumerable<AllDietsViewModel>> AllDiets()
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
    }
}
