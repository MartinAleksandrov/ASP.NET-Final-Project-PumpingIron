namespace Pumping_Iron.Core.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Pumping_Iron.Core.Data.Models;

    public class PumpingIronDbContext : IdentityDbContext
    {
        public PumpingIronDbContext(DbContextOptions<PumpingIronDbContext> options)
            : base(options)
        {
        }


        public DbSet<Trainer> Trainers { get; set; } = null!;

        public DbSet<Client> Clients { get; set; } = null!;

        public DbSet<TrainingProgram> TrainingPrograms { get; set; } = null!;

        public DbSet<Diet> Diets { get; set; } = null!;

        public DbSet<GymParticipants> GymParticipants { get; set; } = null!;

        public int MyProperty { get; set; }
    }
}