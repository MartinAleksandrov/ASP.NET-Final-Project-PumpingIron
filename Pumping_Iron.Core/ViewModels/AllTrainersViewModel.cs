namespace Pumping_Iron.Core.ViewModels
{
    using Pumping_Iron.Data.Models.Enums;
    public class AllTrainersViewModel
    {
        public string Name { get; set; } = string.Empty;

        public int Age { get; set; }

        public Gender Gender { get; set; }

        public string ImageUrl { get; set; } = null!;
    }
}
