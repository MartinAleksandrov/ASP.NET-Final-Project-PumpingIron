namespace Pumping_Iron.Services.Interfaces
{
    using Pumping_Iron.Data.ViewModels.Client;
    using Pumping_Iron.Data.ViewModels.Diet;
    using Pumping_Iron.Data.ViewModels.Trainer;

    public interface ITrainerService
    {
        Task<IEnumerable<AllTrainersViewModel>> AllTrainersAsync();

        Task<TrainerDetailsViewModel?> GetTrainerDetailsAsync(string id);

        Task<string> FindTrainerByIdAsync(string id);

        Task<bool> HireTrainerAsync(HireTrainerViewModel model, string trainerId,string clientId);

        Task<bool> RemoveClient(string clientId, string trainerId);


        Task<IEnumerable<AddProgramViewModel>?> GetAllTrainerPrograms(string trainerId,string clientId);

        Task<bool> AddProgramToClient(int programId,string clientId);

        Task<IEnumerable<AddDietViewModel>?> GetAllTrainerDiets(string trainerId, string clientId);
        Task<bool> AddDietToClient(int dietId, string clientId);

        Task<bool> RemoveProgramFromClientAsync(int programId,string clientId);

        Task<bool> RemoveDietFromClientAsync(int dietId, string clientId);

        Task<bool> IsClientAlreadyHireTrainerAsync(string clientId);

    }
}