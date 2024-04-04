namespace Pumping_Iron.Data.Models
{
    using static Pumping_Iron.Common.EntityValidationsConstants.ProgramConstants;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class TrainingProgram
    {
        public TrainingProgram()
        {
            Clients = new List<Client>();
        }

        [Key]
        public int Id { get; set; }

        
        [Required]
        [MaxLength(ProgramNameMaxLength)]
        public string Name { get; set; } = string.Empty;

        
        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = string.Empty;


        [Required]
        [MaxLength(MaxDuration)]
        public int Duration { get; set; }

        [Required]
        [MaxLength(ImageUrlMaxLength)]
        public string ImageUrl { get; set; } = null!;


        [ForeignKey(nameof(Trainer))] 
        public Guid TrainerId { get; set; }

        public Trainer Trainer { get; set; } = null!; 


        public ICollection<Client> Clients { get; set; }
    }
}