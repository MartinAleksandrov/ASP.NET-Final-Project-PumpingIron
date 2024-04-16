namespace Pumping_Iron.Services.Interfaces
{
    using Pumping_Iron.Data.ViewModels.Client;
    using Pumping_Iron.Data.ViewModels.Trainer;

    public interface IClientService
    {
        Task<IEnumerable<MyClientsViewModel>?> GetMyClientsAsync(string trainerId);

        Task<TrainerDetailsViewModel?> GetMyTrainerInfo(string userId);
        Task<ClientDietViewModel?> GetMyDietInfo(string userId);

        Task<ClientProgramViewModel?> GetMyProgramInfo(string userId);

    }
}