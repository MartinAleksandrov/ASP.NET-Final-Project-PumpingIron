namespace Pumping_Iron.Data.ViewModels
{
    public class AllTrainersViewModel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = string.Empty;

        public int Age { get; set; }

        public string Gender { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;
    }
}
