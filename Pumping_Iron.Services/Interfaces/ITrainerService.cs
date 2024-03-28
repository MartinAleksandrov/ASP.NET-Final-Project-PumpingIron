namespace Pumping_Iron.Services.Interfaces
{
    using Pumping_Iron.Data.ViewModels;

    public interface ITrainerService
    {
        Task<IEnumerable<AllTrainersViewModel>> AllTrainersAsync();
    }
}
