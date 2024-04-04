namespace Pumping_Iron.Data.ViewModels.TrainingPrograms
{
    using System.ComponentModel.DataAnnotations;
    using static Pumping_Iron.Common.EntityValidationsConstants.ProgramConstants;
    public class CreateProgramViewModel
    {
        public int Id { get; set; }


        [Required]
        [StringLength(ProgramNameMaxLength,MinimumLength =ProgramNameMinLength)]
        public string Name { get; set; } = string.Empty;


        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        public string Description { get; set; } = string.Empty;


        [Required]
        [Range(MinDuration,MaxDuration)]
        public int Duration { get; set; }

        [Required]
        [MaxLength(ImageUrlMaxLength)]
        public string ImageUrl { get; set; } = null!;
    }
}
