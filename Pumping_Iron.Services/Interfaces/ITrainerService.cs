namespace Pumping_Iron.Services.Interfaces
{
    using Pumping_Iron.Data.ViewModels.Trainer;

    public interface ITrainerService
    {
        Task<IEnumerable<AllTrainersViewModel>> AllTrainersAsync();

        Task<TrainerDetailsViewModel?> GetTrainerDetailsAsync(string id);

        Task<string> FindTrainerByIdAsync(string id);

    }
}
