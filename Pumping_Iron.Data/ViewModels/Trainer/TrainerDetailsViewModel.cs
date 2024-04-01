namespace Pumping_Iron.Data.ViewModels.Trainer
{
    public class TrainerDetailsViewModel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = string.Empty;

        public string? Information { get; set; }
        public string ImageUrl { get; set; } = null!;
    }
}
