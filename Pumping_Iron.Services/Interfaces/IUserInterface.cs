namespace Pumping_Iron.Services.Interfaces
{
    public interface IUserInterface
    {
        Task<bool> IsUserTrainer(string Id);
    }
}
