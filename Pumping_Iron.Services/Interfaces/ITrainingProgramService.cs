namespace Pumping_Iron.Services.Interfaces
{
    using Pumping_Iron.Data.ViewModels.TrainingPrograms;
    public interface ITrainingProgramService
    {
        Task<IEnumerable<AllTrainingProgramsViewModel>> AllTrainingProgramsAsync();

        Task<AllTrainingProgramsViewModel?> GetProgramDetailsAsync(int id);

        Task<bool> ExistByIdAsync(int id);

        Task<bool> CreateProgramAsync(CreateProgramViewModel model,string trainerId);

    }
}