﻿namespace Pumping_Iron.Data.ViewModels.Client
{
    public class ClientDietViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = null!;
    }
}