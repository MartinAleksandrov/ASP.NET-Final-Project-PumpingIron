﻿namespace Pumping_Iron.Data.ViewModels.Diet
{
    public class AddDietViewModel
    {
        public string ClientId { get; set; } = string.Empty;

        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = null!;

    }
}
