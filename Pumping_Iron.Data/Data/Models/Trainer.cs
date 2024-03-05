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


        public ICollection<Client> Clients { get; set; }

    }
}
