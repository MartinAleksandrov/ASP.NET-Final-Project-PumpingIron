namespace Pumping_Iron.Data.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Pumping_Iron.Data.Models;

    public class PumpingIronDbContext : IdentityDbContext
    {
      
        public PumpingIronDbContext() 
        {

        }

        public PumpingIronDbContext(DbContextOptions<PumpingIronDbContext> options)
            : base(options)
        {
        }
       
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<GymParticipants>().HasKey(gp => new { gp.ClientId, gp.ParticipantId });

            base.OnModelCreating(builder);
        }

        public DbSet<Trainer> Trainers { get; set; } = null!;

        public DbSet<Client> Clients { get; set; } = null!;

        public DbSet<TrainingProgram> TrainingPrograms { get; set; } = null!;

        public DbSet<Diet> Diets { get; set; } = null!;

        public DbSet<GymParticipants> GymParticipants { get; set; } = null!;

        public DbSet<Membership> Memberships { get; set; } = null!;
    }
}