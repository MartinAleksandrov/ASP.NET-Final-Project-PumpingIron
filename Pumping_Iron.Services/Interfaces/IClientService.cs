namespace Pumping_Iron.Services.Interfaces
{
    using Pumping_Iron.Data.ViewModels.Client;
    using Pumping_Iron.Data.ViewModels.Trainer;

    public interface IClientService
    {
        Task<PaginatedList<MyClientsViewModel>?> GetMyClientsAsync(string trainerId, int pageNumber, int pageSize);

        Task<TrainerDetailsViewModel?> GetMyTrainerInfo(string userId);
        Task<ClientDietViewModel?> GetMyDietInfo(string userId);

        Task<ClientProgramViewModel?> GetMyProgramInfo(string userId);

        Task<bool> RemoveMyTrainer(string clientId);

    }
}