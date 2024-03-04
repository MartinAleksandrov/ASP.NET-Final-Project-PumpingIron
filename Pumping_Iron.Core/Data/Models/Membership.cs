namespace Pumping_Iron.Core.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Membership
    {
        public Membership() 
        {
            MembershipId = Guid.NewGuid();
            Members = new List<Client>();
        }

        [Key]
        public Guid MembershipId { get; set; }


        [Required]
        public List<Client> Members { get; set; } = null!;

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}