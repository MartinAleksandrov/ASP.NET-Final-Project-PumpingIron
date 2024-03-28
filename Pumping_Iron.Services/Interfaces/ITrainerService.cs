namespace Pumping_Iron.Services.Interfaces
{
    using Pumping_Iron.Core.ViewModels;

    public interface ITrainerService
    {
        Task<IEnumerable<AllTrainersViewModel>> AllTrainersAsync();
    }
}
