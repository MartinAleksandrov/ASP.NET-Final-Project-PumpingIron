namespace Pumping_Iron.Data.Models
{
    using static Pumping_Iron.Common.EntityValidationsConstants.DietConstants;
    using System.ComponentModel.DataAnnotations;

    public class Diet
    {
        public Diet()
        {
            Clients = new List<Client>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(DietNameMaxLength)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(DietDescriptionMaxLength)]
        public string Description { get; set; } = string.Empty;

        
        public List<Client> Clients { get; set; } 
    }
}