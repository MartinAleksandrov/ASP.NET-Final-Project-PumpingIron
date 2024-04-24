namespace Pumping_Iron.Data.ViewModels.Client
{
    using Pumping_Iron.Data.Models;
    using Pumping_Iron.Data.Models.Enums;
    using System.ComponentModel.DataAnnotations;
    using static Pumping_Iron.Common.EntityValidationsConstants.ClientConstants;
    public class HireTrainerViewModel
    {
        public Guid ClientId { get; set; }

        public Guid TrainerId { get; set; }


        [Required(ErrorMessage = "Name is required")]
        [StringLength(ClientNameMaxLength,MinimumLength = ClientNameMinLength)]
        public string Name { get; set; } = string.Empty;


        [Required(ErrorMessage = "Age is required")]
        [Range(ClientMinAge,ClientMaxAge)]
        public int Age { get; set; }

        [Required(ErrorMessage = "Image URL is required")]
        [MaxLength(ImageUrlMaxLength)]
        public string ImageUrl { get; set; } = string.Empty;

        [Required(ErrorMessage = "Gender is required")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Membership type is required")]
        public Membership Membership { get; set; } = null!;
    } 
}