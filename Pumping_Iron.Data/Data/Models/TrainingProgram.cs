namespace Pumping_Iron.Data.Models
{
    using static Pumping_Iron.Common.EntityValidationsConstants.ProgramConstants;
    using System.ComponentModel.DataAnnotations;
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


        public ICollection<Client> Clients { get; set; }
    }
}