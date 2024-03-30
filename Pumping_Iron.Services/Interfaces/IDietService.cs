namespace Pumping_Iron.Services.Interfaces
{
    using Pumping_Iron.Data.ViewModels.Diet;
    public interface IDietService
    {
        Task<IEnumerable<AllDietsViewModel>> AllDiets();
    }
}
