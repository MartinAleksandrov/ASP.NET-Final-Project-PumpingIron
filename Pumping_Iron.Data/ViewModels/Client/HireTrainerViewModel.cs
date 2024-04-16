namespace Pumping_Iron.Data.ViewModels.Client
{
    using Pumping_Iron.Data.Data.Models.Enums;
    using Pumping_Iron.Data.Models;
    using Pumping_Iron.Data.Models.Enums;
    using System.ComponentModel.DataAnnotations;
    using static Pumping_Iron.Common.EntityValidationsConstants.ClientConstants;
    public class HireTrainerViewModel
    {
        public Guid ClientId { get; set; }

        public Guid TrainerId { get; set; }


        [Required]
        [StringLength(ClientNameMaxLength,MinimumLength = ClientNameMinLength)]
        public string Name { get; set; } = string.Empty;


        [Required]
        [Range(ClientMinAge,ClientMaxAge)]
        public int Age { get; set; }

        [Required]
        [MaxLength(ImageUrlMaxLength)]
        public string ImageUrl { get; set; } = string.Empty;

        [Required]
        public Gender Gender { get; set; }

        [Required]
        public Membership Membership { get; set; } = null!;
    } 
}