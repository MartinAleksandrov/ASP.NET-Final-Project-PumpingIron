namespace Pumping_Iron.Data.Models
{
    using static Pumping_Iron.Common.EntityValidationsConstants.ClientConstants;
    using System.ComponentModel.DataAnnotations;
    using Pumping_Iron.Data.Models.Enums;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Client
    {

        public Client()
        {
            ClientId = Guid.NewGuid();
        }

        [Key]
        public Guid ClientId { get; set; }


        [Required]
        [MaxLength(ClientNameMaxLength)]
        public string Name { get; set; } = string.Empty;


        [Required]
        [MaxLength(ClientMaxAge)]
        public int Age { get; set; }

        [Required]
        [MaxLength(ImageUrlMaxLength)]
        public string ImageUrl { get; set; } = string.Empty;

        [Required]
        public Gender Gender { get; set; }


        [ForeignKey(nameof(TrainingProgram))]
        public int? TrainingProgramId { get; set; }

        public TrainingProgram? TrainingProgram { get; set; }


        [ForeignKey(nameof(Diet))]
        public int? DietId { get; set; }

        public Diet? Diet { get; set; } 


        [ForeignKey(nameof(Trainer))]
        public Guid? TrainerId { get; set; }

        public Trainer? Trainer { get; set; } 


        [ForeignKey(nameof(Membership))]
        public int? MembershipId { get; set; }

        public Membership? Membership { get; set; } = null!;
    }
}