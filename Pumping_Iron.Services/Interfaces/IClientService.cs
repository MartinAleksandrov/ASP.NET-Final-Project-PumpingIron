namespace Pumping_Iron.Services.Interfaces
{
    using Pumping_Iron.Data.ViewModels.Client;
    public interface IClientService
    {
        Task<IEnumerable<MyClientsViewModel>?> GetMyClientsAsync(string trainerId);


    }
}