namespace Pumping_Iron.Services
{
    using Pumping_Iron.Data.Data;
    using Pumping_Iron.Data.ViewModels.Client;
    using Pumping_Iron.Services.Interfaces;
    using System.Threading.Tasks;

    public class ClientService : IClientService
    {
        private readonly PumpingIronDbContext dbContext;
        private readonly ITrainerService trainerService;

        public ClientService(PumpingIronDbContext context,ITrainerService service)
        {
            dbContext = context;
            trainerService = service;
        }

        public async Task<IEnumerable<MyClientsViewModel>?> GetMyClientsAsync(string trainerId)
        {
            var isTrainerExist = await trainerService.FindTrainerByIdAsync(trainerId);

            if (string.IsNullOrEmpty(isTrainerExist))
            {
                return null;
            }

            throw new NotImplementedException();
        }
    }
}