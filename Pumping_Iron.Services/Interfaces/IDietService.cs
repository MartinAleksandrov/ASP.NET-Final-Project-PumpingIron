namespace Pumping_Iron.Services.Interfaces
{
    using Pumping_Iron.Data.ViewModels.Diet;
    using Pumping_Iron.Data.ViewModels.TrainingPrograms;

    public interface IDietService
    {
        Task<IEnumerable<AllDietsViewModel>> AllDietsAsync();

        Task<AllDietsViewModel?> GetDetaisByIdAsync(int id);

        Task<bool> ExistByIdAsync(int id);
    }
}
