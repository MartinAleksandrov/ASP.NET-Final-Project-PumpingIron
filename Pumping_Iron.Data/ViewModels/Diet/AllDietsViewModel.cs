namespace Pumping_Iron.Data.ViewModels.Diet
{
    public class AllDietsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = null!;
    }
}