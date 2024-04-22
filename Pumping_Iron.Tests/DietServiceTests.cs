namespace Pumping_Iron.Tests
{
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Pumping_Iron.Data.Data;
    using Pumping_Iron.Data.Models;
    using Pumping_Iron.Data.ViewModels.Diet;
    using Pumping_Iron.Services;
    using Pumping_Iron.Services.Interfaces;

    public class DietServiceTests
    {
        private PumpingIronDbContext dbContextMock;
        private Mock<ITrainerService> trainerServiceMock;
        private DbContextOptions<PumpingIronDbContext> options;
        private IDietService dietService;


        [SetUp]
        public void Setup()
        {
            options = new DbContextOptionsBuilder<PumpingIronDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            dbContextMock = new PumpingIronDbContext(options);

            trainerServiceMock = new Mock<ITrainerService>();

            dietService = new DietService(dbContextMock, trainerServiceMock.Object);
        }

        [Test]
        public async Task AllDietsAsyncMustReturnAllDietsViewModels()
        {
            var diets = new List<Diet>
            {
             new Diet { Id = 1, Name = "Diet 1", Description = "Description 1", ImageUrl = "www.somepic.com" },
             new Diet { Id = 2, Name = "Diet 2", Description = "Description 2", ImageUrl = "www.somepic.com" }
            };

            dbContextMock.Diets.AddRange(diets);
            dbContextMock.SaveChanges();

            var result = await dietService.AllDietsAsync();

            Assert.IsNotNull(result);
            Assert.That(result.Count(), Is.EqualTo(diets.Count));

            for (int i = 0; i < diets.Count; i++)
            {
                var expected = new AllDietsViewModel
                {
                    Id = diets[i].Id,
                    Name = diets[i].Name,
                    Description = diets[i].Description,
                    ImageUrl = diets[i].ImageUrl
                };

                var actual = result.ElementAt(i);

                Assert.That(actual.Id, Is.EqualTo(expected.Id));
                Assert.That(actual.Name, Is.EqualTo(expected.Name));
                Assert.That(actual.Description, Is.EqualTo(expected.Description));
                Assert.That(actual.ImageUrl, Is.EqualTo(expected.ImageUrl));
            }
        }

        [Test]
        public async Task CreateDietAsyncIfDietAlreadyExistsReturnsFalse()
        {
            var model = new CreateDietViewModel
            {
                Id = 1,
                Name = "Existing Diet",
                Description = "Description",
                ImageUrl = "www.pic.com"
            };
            var trainerId = Guid.NewGuid();

            dbContextMock.Diets.Add(new Diet { Id = 1, Name = "Existing Diet",
                ImageUrl = "www.pic.com"
            });
            await dbContextMock.SaveChangesAsync();

            var result = await dietService.CreateDietAsync(model, trainerId.ToString());

            Assert.IsFalse(result);
        }

        [Test]
        public async Task CreateDietAsyncIfTrainerNotFoundReturnsFalse()
        {
            var model = new CreateDietViewModel
            {
                Name = "New Diet",
                Description = "Description",
                ImageUrl = "www.pic.com"
            };
            var trainerId = Guid.NewGuid();

            var result = await dietService.CreateDietAsync(model, trainerId.ToString());

            Assert.IsFalse(result);
        }

        [Test]
        public async Task CreateDietAsyncIfDietCreatedSuccessfullyReturnsTrue()
        {
            var model = new CreateDietViewModel
            {
                Name = "New Diet",
                Description = "Description",
                ImageUrl = "Image URL"
            };
            var trainerId = Guid.NewGuid();

            dbContextMock.Trainers.Add(new Trainer { TrainerId = trainerId, ImageUrl ="www.pic.com" });
            await dbContextMock.SaveChangesAsync();

            var result = await dietService.CreateDietAsync(model, trainerId.ToString());

            Assert.IsTrue(result);
            Assert.IsTrue(dbContextMock.Diets.Any(d => d.Name == model.Name));
        }

        [Test]
        public async Task ExistByIdAsyncIfDietExistsReturnsTrue()
        {
            var existingDiet = new Diet { Id = 1, Name = "Diet",ImageUrl = "www.some.com" };
            dbContextMock.Diets.Add(existingDiet);
            await dbContextMock.SaveChangesAsync();

            var result = await dietService.ExistByIdAsync(existingDiet.Id);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task ExistByIdAsyncIfDietDoesNotExistReturnsFalse()
        {
            var dietId = 1;

            var result = await dietService.ExistByIdAsync(dietId);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetDetailsByIdAsyncIfDietExistsReturnsAllDietsViewModel()
        {
            var diet = new Diet { Id = 1, Name = "Diet", Description = "Description", ImageUrl = "www.somepic.com" };
            dbContextMock.Diets.Add(diet);
            await dbContextMock.SaveChangesAsync();

            var result = await dietService.GetDetailsByIdAsync(diet.Id);

            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(diet.Id));
            Assert.That(result.Name, Is.EqualTo(diet.Name));
            Assert.That(result.Description, Is.EqualTo(diet.Description));
            Assert.That(result.ImageUrl, Is.EqualTo(diet.ImageUrl));
        }

        [Test]
        public async Task GetDetailsByIdAsyncIfDietDoesNotExistReturnsNull()
        {
            var dietId = 1;

            var result = await dietService.GetDetailsByIdAsync(dietId);

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetMyDietsAsyncIfTrainerExistsReturnsMyDietsViewModels()
        {
            var trainerId = Guid.NewGuid();

            var trainerDietsData = new List<Diet>
            {
            new Diet { Id = 1, Name = "Diet 1", Description = "Description 1", ImageUrl = "www.somepic1.com", TrainerId = trainerId },
            new Diet { Id = 2, Name = "Diet 2", Description = "Description 2", ImageUrl = "www.somepic1.com", TrainerId = trainerId }
            };

            dbContextMock.Diets.AddRange(trainerDietsData);
            await dbContextMock.SaveChangesAsync();

            trainerServiceMock.Setup(t => t.FindTrainerByIdAsync(trainerId.ToString())).ReturnsAsync(trainerId.ToString());

            var result = await dietService.GetMyDietsAsync(trainerId.ToString());

            Assert.IsNotNull(result);
            Assert.That(result.Count(), Is.EqualTo(trainerDietsData.Count));

            for (int i = 0; i < trainerDietsData.Count; i++)
            {
                var expected = new MyDietsViewModel
                {
                    Id = trainerDietsData[i].Id,
                    Name = trainerDietsData[i].Name,
                    Description = trainerDietsData[i].Description,
                    ImageUrl = trainerDietsData[i].ImageUrl
                };

                var actual = result.ElementAt(i);

                Assert.That(actual.Id, Is.EqualTo(expected.Id));
                Assert.That(actual.Name, Is.EqualTo(expected.Name));
                Assert.That(actual.Description, Is.EqualTo(expected.Description));
                Assert.That(actual.ImageUrl, Is.EqualTo(expected.ImageUrl));
            }
        }

        [Test]
        public async Task GetMyDietsAsyncIfTrainerDoesNotExistReturnsNull()
        {
            var trainerId = Guid.NewGuid();

            trainerServiceMock.Setup(t => t.FindTrainerByIdAsync(trainerId.ToString())).ReturnsAsync((string)null!);

            var result = await dietService.GetMyDietsAsync(trainerId.ToString());

            Assert.IsNull(result);
        }
    }
}