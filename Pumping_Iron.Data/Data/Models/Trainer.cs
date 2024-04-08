namespace Pumping_Iron.Data.Models
{
    using Pumping_Iron.Data.Models.Enums;
    using System.ComponentModel.DataAnnotations;
    using static Pumping_Iron.Common.EntityValidationsConstants.TrainerConstants;

    public class Trainer
    {
        public Trainer()
        {
            TrainerId = Guid.NewGuid();
            Clients = new List<Client>();
            TrainingPrograms = new List<TrainingProgram>();
            Diets = new List<Diet>();
        }

        [Key]
        public Guid TrainerId { get; set; }


        [Required]
        [MaxLength(TrainerNameMaxLength)]
        public string Name { get; set; } = string.Empty;


        [Required]
        [MaxLength(TrainerMaxAge)]
        public int Age { get; set; }


        [Required]
        public Gender Gender { get; set; }

        [MaxLength(InfoMaxLength)]
        public string? Information { get; set; }

        [Required]
        [MaxLength(ImageUrlMaxLength)]
        public string ImageUrl { get; set; } = null!;

        public ICollection<Client> Clients { get; set; }

        public ICollection<TrainingProgram> TrainingPrograms { get; set; }

        public ICollection<Diet> Diets { get; set; }

    }
}
