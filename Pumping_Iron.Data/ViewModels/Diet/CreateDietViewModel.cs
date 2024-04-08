namespace Pumping_Iron.Data.ViewModels.Diet
{
    using System.ComponentModel.DataAnnotations;
    using static Pumping_Iron.Common.EntityValidationsConstants.DietConstants;
    public class CreateDietViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(DietNameMaxLength)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(DietDescriptionMaxLength)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [MaxLength(ImageUrlMaxLength)]
        public string ImageUrl { get; set; } = null!;
    }
}