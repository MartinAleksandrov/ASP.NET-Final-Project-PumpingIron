namespace Pumping_Iron.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
 
    public class GymParticipants
    {
        [Required]
        [ForeignKey(nameof(Client))]
        public Guid ClientId { get; set; }

        [Required]
        public Client Client { get; set; } = null!;


        [Required]
        [ForeignKey(nameof(Participant))]
        public string ParticipantId { get; set; } = string.Empty;

        [Required]
        public IdentityUser Participant { get; set; } = null!;
    }
}