namespace Pumping_Iron.Data.ViewModels.Client
{
    public class MyClientsViewModel
    {
        public Guid ClientId { get; set; }
        
        public string Name { get; set; } = string.Empty;

        public int Age { get; set; }

        public string Gender { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;

        public ClientProgramViewModel TrainingProgram { get; set; } = null!;


        public ClientDietViewModel Diet { get; set; } = null!;


        public string MembershipName { get; set; } = null!;
    }
}