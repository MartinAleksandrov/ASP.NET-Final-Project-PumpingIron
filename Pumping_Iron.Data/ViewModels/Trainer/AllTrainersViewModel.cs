﻿namespace Pumping_Iron.Data.ViewModels.Trainer
{
    public class AllTrainersViewModel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = string.Empty;

        public int Age { get; set; }

        public string Gender { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;
    }
}
