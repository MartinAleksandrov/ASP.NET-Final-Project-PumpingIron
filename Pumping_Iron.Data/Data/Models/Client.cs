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
        public Gender Gender { get; set; }


        [Required]
        [ForeignKey(nameof(TrainingProgram))]
        public int TrainingProgramId { get; set; }

        [Required]
        public TrainingProgram TrainingProgram { get; set; } = null!;


        [Required]
        [ForeignKey(nameof(Diet))]
        public int DietId { get; set; }

        [Required]
        public Diet Diet { get; set; } = null!;


        [Required]
        [ForeignKey(nameof(Trainer))]
        public Guid TrainerId { get; set; }

        [Required]
        public Trainer Trainer { get; set; } = null!;


        [Required]
        [ForeignKey(nameof(Membership))]
        public int MembershipId { get; set; }

        [Required]
        public Membership Membership { get; set; } = null!;
    }
}