namespace Pumping_Iron.Services.Interfaces
{
    using Pumping_Iron.Data.ViewModels.TrainingPrograms;
    public interface ITrainingProgramService
    {
        Task<IEnumerable<AllTrainingProgramsViewModel>> AllTrainingProgramsAsync();

    }
}