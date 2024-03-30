namespace Pumping_Iron.Data.ViewModels.TrainingPrograms
{
    public class AllTrainingProgramsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int Duration { get; set; }

        public string ImageUrl { get; set; } = null!;
    }
}
