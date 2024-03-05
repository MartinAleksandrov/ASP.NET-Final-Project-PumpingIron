namespace Pumping_Iron.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Membership
    {
        public Membership() 
        {
            Members = new List<Client>();
        }

        [Key]
        public int MembershipId { get; set; }


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