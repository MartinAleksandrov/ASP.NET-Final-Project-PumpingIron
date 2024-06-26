﻿namespace Pumping_Iron.Core.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Pumping_Iron.Data.ViewModels.Diet;
    using Pumping_Iron.Infrastructure.Extensions;
    using Pumping_Iron.Services;
    using Pumping_Iron.Services.Interfaces;

    public class DietController : Controller
    {
        private readonly IDietService dietService;

        public DietController(IDietService service)
        {
            dietService = service;
        }

        [HttpGet]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> AllDiets(string search)
        {
            IEnumerable<AllDietsViewModel> allDiets;

            if (!string.IsNullOrEmpty(search))
            {
                // Filter diets by search query
                allDiets = await dietService.SearchDietsAsync(search);
            }
            else
            {
                // If no search query provided, fetch all diets
                allDiets = await dietService.AllDietsAsync();
            }

            return View(allDiets);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            // Retrieve diet details by ID asynchronously
            var model = await dietService.GetDetailsByIdAsync(id);

            // If no diet details are found for the given ID, return a BadRequest response
            if (model == null)
            {
                return BadRequest();
            }

            // If diet details are found, return the view with the model data
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Coach")]
        public IActionResult CreateDiet()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Coach")]
        public async Task<IActionResult> CreateDiet(CreateDietViewModel viewModel)
        {
            var userId = User.GetId();

            try
            {
                var isCreated = await dietService.CreateDietAsync(viewModel, userId);
                if (!isCreated)
                {
                    ModelState.AddModelError(string.Empty, "Failed to create the diet. It may already exist or there was an issue with the provided data.");
                    return View(viewModel); // Return the view with the model to display validation errors
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later or contact the administrator.");
                return View(viewModel); // Return the view with the model to display the error message
            }

            return RedirectToAction(nameof(TrainerDiets));

        }


        [HttpGet]
        [Authorize(Roles = "Coach")]
        public async Task<IActionResult> TrainerDiets()
        {
            // Get the current user's ID
            var userId = User.GetId();

            // Retrieve diets associated with the current user (trainer)
            var diets = await dietService.GetMyDietsAsync(userId);

            // Check if diets were successfully retrieved
            if (diets == null)
            {
                // Return a BadRequest response if diets are null (Trainer does not exist or error occurred)
                return BadRequest("Trainer does not exist or error occurred.");
            }

            // Return the TrainerDiets view with the retrieved diets
            return View(diets);
        }
    }
}