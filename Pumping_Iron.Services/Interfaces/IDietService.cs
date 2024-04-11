namespace Pumping_Iron.Services.Interfaces
{
    using Pumping_Iron.Data.ViewModels.Diet;
    public interface IDietService
    {
        Task<IEnumerable<AllDietsViewModel>> AllDietsAsync();

        Task<AllDietsViewModel?> GetDetaisByIdAsync(int id);

        Task<bool> ExistByIdAsync(int id);

        Task<bool> CreateDietAsync(CreateDietViewModel model, string trainerId);

        Task<IEnumerable<MyDietsViewModel>?> GetMyDietsAsync(string trainerId);

    }
}