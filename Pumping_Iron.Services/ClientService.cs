namespace Pumping_Iron.Services
{
    using Microsoft.EntityFrameworkCore;
    using Pumping_Iron.Data.Data;
    using Pumping_Iron.Data.ViewModels.Client;
    using Pumping_Iron.Services.Interfaces;
    using System.Threading.Tasks;

    public class ClientService : IClientService
    {
        private readonly PumpingIronDbContext dbContext;
        private readonly ITrainerService trainerService;

        public ClientService(PumpingIronDbContext context, ITrainerService service)
        {
            dbContext = context;
            trainerService = service;
        }

        /// <summary>
        /// Retrieves the clients associated with a trainer identified by the provided trainerId.
        /// </summary>
        /// <param name="trainerId">The ID of the trainer.</param>
        /// <returns>
        /// A collection of MyClientsViewModel representing the clients associated with the trainer.
        /// Returns null if the trainer does not exist.
        /// </returns>
        public async Task<IEnumerable<MyClientsViewModel>?> GetMyClientsAsync(string trainerId)
        {
            // Check if the trainer exists
            var isTrainerExist = await trainerService.FindTrainerByIdAsync(trainerId);

            // If the trainer does not exist, return null
            if (string.IsNullOrEmpty(isTrainerExist))
            {
                return null;
            }

            // Retrieve the clients associated with the trainer
            var myClients = await dbContext.Clients
                .AsNoTracking()
                .Where(c => c.TrainerId.ToString() == trainerId)
                .Include(c => c.Membership)
                .Include(c => c.TrainingProgram)
                .Select(c => new MyClientsViewModel
                {
                    // Map client properties to MyClientsViewModel properties
                    Name = c.Name,
                    Age = c.Age,
                    Gender = c.Gender.ToString(),
                    ImageUrl = c.ImageUrl,
                    MembershipName = c.Membership!.TypeMembership.ToString()

                    //TrainingProgram = new ClientProgramViewModel
                    //{
                    //    // Map training program properties to ClientProgramViewModel properties
                    //    Id = c.TrainingProgram.Id,
                    //    Name = c.TrainingProgram.Name,
                    //    Description = c.TrainingProgram.Description,
                    //    Duration = c.TrainingProgram.Duration,
                    //    ImageUrl = c.TrainingProgram.ImageUrl
                    //}
                })
                .ToListAsync();

            // Return the collection of clients associated with the trainer
            return myClients;
        }

       
       
    }
}