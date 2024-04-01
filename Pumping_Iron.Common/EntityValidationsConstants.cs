namespace Pumping_Iron.Common
{
    public static class EntityValidationsConstants
    {
        public static class TrainerConstants
        {
            public const int TrainerNameMinLength = 3;
            public const int TrainerNameMaxLength = 30;

            public const int TrainerMinAge = 18;
            public const int TrainerMaxAge = 65;

            public const int InfoMaxLength = 1024;


            public const int ImageUrlMaxLength = 255;
        }

        public static class ClientConstants
        {
            public const int ClientNameMinLength = 3;
            public const int ClientNameMaxLength = 30;

            public const int ClientMinAge = 12;
            public const int ClientMaxAge = 100;
        }

        public static class ProgramConstants
        {
            public const int ProgramNameMinLength = 3;
            public const int ProgramNameMaxLength = 25;

            public const int DescriptionMinLength = 5;
            public const int DescriptionMaxLength = 500;

            public const int MaxDuration = 240;
            public const int MinDuration = 30;

            public const int ImageUrlMaxLength = 255;
        }

        public static class DietConstants
        {
            public const int DietNameMinLength = 3;
            public const int DietNameMaxLength = 25;

            public const int DietDescriptionMinLength = 5;
            public const int DietDescriptionMaxLength = 500;

            public const int ImageUrlMaxLength = 255;

        }

        public static class GymMembershipConstants
        {
            public const string DateFormat = "dd-MM-yyyy";
        }
    }
}