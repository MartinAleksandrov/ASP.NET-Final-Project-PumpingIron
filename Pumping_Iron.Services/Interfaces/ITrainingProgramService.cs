namespace Pumping_Iron.Services.Interfaces
{
    using Pumping_Iron.Core.ViewModels;

    public interface ITrainingProgramService
    {
        Task<IEnumerable<AllTrainersViewModel>> AllTrainersAsync();
    }
}
