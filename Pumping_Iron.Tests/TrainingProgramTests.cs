namespace Pumping_Iron.Tests
{
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Pumping_Iron.Data.Data;
    using Pumping_Iron.Data.Models;
    using Pumping_Iron.Data.ViewModels.TrainingPrograms;
    using Pumping_Iron.Services;
    using Pumping_Iron.Services.Interfaces;

    public class TrainingProgramTests
    {
        private PumpingIronDbContext dbContextMock;
        private Mock<ITrainerService> trainerServiceMock;
        private DbContextOptions<PumpingIronDbContext> options;
        private ITrainingProgramService trainingProgramServiceMock;


        [SetUp]
        public void Setup()
        {
            options = new DbContextOptionsBuilder<PumpingIronDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            dbContextMock = new PumpingIronDbContext(options);

            trainerServiceMock = new Mock<ITrainerService>();

            trainingProgramServiceMock = new TrainingProgramService(dbContextMock, trainerServiceMock.Object);
        }

        [Test]
        public async Task ExistByIdAsyncReturnsFalseIfProgramDoesNotExist()
        {
            var exist = await dbContextMock.TrainingPrograms.AnyAsync(tp => tp.Id == 1);

            Assert.IsFalse(exist);
        }

        [Test]
        public async Task ExistByIdAsyncReturnsTrueIfProgramExist()
        {
            var program = new TrainingProgram()
            {
                Id = 1,
                Description = "Test",
                Duration = 10,
                Name = "PPL",
                ImageUrl = "www.pic.com",
                TrainerId = Guid.NewGuid()
            };

            await dbContextMock.TrainingPrograms.AddAsync(program);
            await dbContextMock.SaveChangesAsync();

            var exist = await dbContextMock.TrainingPrograms.AnyAsync(tp => tp.Id == program.Id);

            Assert.IsTrue(exist);
        }

        [Test]
        public async Task GetMyTrainingProgramsAsyncIfTrainerExistsReturnsTrainingPrograms()
        {
            var trainerId = Guid.NewGuid();
            var trainer = new Trainer { TrainerId = trainerId, ImageUrl = "www.pic.com" };
            dbContextMock.Trainers.Add(trainer);
            await dbContextMock.SaveChangesAsync();

            trainerServiceMock.Setup(x => x.FindTrainerByIdAsync(trainerId.ToString())).ReturnsAsync(trainerId.ToString());

            var expectedPrograms = new List<MyTrainingPrograms>
                     {
                         new MyTrainingPrograms { Id = 1, Name = "Program 1",ImageUrl = "www.pic.com" },
                         new MyTrainingPrograms { Id = 2, Name = "Program 2",ImageUrl = "www.ppp.com" }
                     };

            dbContextMock.TrainingPrograms.AddRange(expectedPrograms.Select(p =>
                new TrainingProgram
                {
                    Id = p.Id,
                    Name = p.Name,
                    TrainerId = trainerId,
                    ImageUrl = "www.p21.com"
                }));
            await dbContextMock.SaveChangesAsync();


            var result = await trainingProgramServiceMock.GetMyTrainingProgramsAsync(trainerId.ToString());

            Assert.IsNotNull(result);
            Assert.That(result.Count(), Is.EqualTo(expectedPrograms.Count));
            Assert.IsTrue(expectedPrograms.All(p => result.Any(r => r.Id == p.Id && r.Name == p.Name)));
        }

        [Test]
        public async Task GetMyTrainingProgramsAsyncIfTrainerDoesNotExistReturnsNull()
        {
            var trainerId = Guid.NewGuid();
            trainerServiceMock.Setup(x => x.FindTrainerByIdAsync(trainerId.ToString())).ReturnsAsync((string)null!);

            var result = await trainingProgramServiceMock.GetMyTrainingProgramsAsync(trainerId.ToString());

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetProgramDetailsAsyncIdProgramExistsReturnsProgramDetails()
        {
            var existingProgram = new TrainingProgram
            {
                Id = 1,
                Name = "Program 1",
                Description = "Description of Program 1",
                Duration = 10,
                ImageUrl = "www.bestpic.com"
            };
            dbContextMock.TrainingPrograms.Add(existingProgram);
            await dbContextMock.SaveChangesAsync();

            var result = await trainingProgramServiceMock.GetProgramDetailsAsync(existingProgram.Id);

            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(existingProgram.Id));
            Assert.That(result.Name, Is.EqualTo(existingProgram.Name));
            Assert.That(result.Description, Is.EqualTo(existingProgram.Description));
            Assert.That(result.Duration, Is.EqualTo(existingProgram.Duration));
            Assert.That(result.ImageUrl, Is.EqualTo(existingProgram.ImageUrl));
        }

        [Test]
        public async Task GetProgramDetailsAsyncIfProgramDoesNotExistReturnsNull()
        {
            var result = await trainingProgramServiceMock.GetProgramDetailsAsync(1);
            Assert.IsNull(result);
        }

        [Test]
        public async Task CreateProgramAsyncNewProgramIsSuccessfullyCreated()
        {
            var trainer = new Trainer { TrainerId = Guid.NewGuid(), ImageUrl = "www.somepic.com" };
            dbContextMock.Trainers.Add(trainer);
            await dbContextMock.SaveChangesAsync();

            var model = new CreateProgramViewModel
            {
                Id = 1,
                Name = "New Program",
                Description = "Description of New Program",
                Duration = 10,
                ImageUrl = "newprogram.jpg"
            };

            var result = await trainingProgramServiceMock.CreateProgramAsync(model, trainer.TrainerId.ToString());

            Assert.IsTrue(result);

            var createdProgram = await dbContextMock.TrainingPrograms.FirstOrDefaultAsync(p => p.Name == model.Name);
            Assert.IsNotNull(createdProgram);
            Assert.That(createdProgram.Id, Is.EqualTo(model.Id));
            Assert.That(createdProgram.Name, Is.EqualTo(model.Name));
            Assert.That(createdProgram.Description, Is.EqualTo(model.Description));
            Assert.That(createdProgram.Duration, Is.EqualTo(model.Duration));
            Assert.That(createdProgram.ImageUrl, Is.EqualTo(model.ImageUrl));
            Assert.That(createdProgram.Trainer, Is.EqualTo(trainer));
        }

        [Test]
        public async Task CreateProgramAsyncIfProgramAlreadyExistsReturnsFalse()
        {
            var trainer = new Trainer { TrainerId = Guid.NewGuid(), ImageUrl = "www.somepic.com" };
            dbContextMock.Trainers.Add(trainer);
            await dbContextMock.SaveChangesAsync();

            var existingProgram = new TrainingProgram
            {
                Id = 1,
                Name = "Existing Program",
                Description = "Description of Existing Program",
                Duration = 10,
                ImageUrl = "www.pic.com",
                Trainer = trainer
            };
            dbContextMock.TrainingPrograms.Add(existingProgram);
            await dbContextMock.SaveChangesAsync();

            var model = new CreateProgramViewModel
            {
                Id = 1,
                Name = "Existing Program", 
                Description = "Description of New Program",
                Duration = 10,
                ImageUrl = "www.pic1.com",
            };

            var result = await trainingProgramServiceMock.CreateProgramAsync(model, Guid.NewGuid().ToString());

            Assert.IsFalse(result);
        }

        [Test]
        public async Task CreateProgramAsyncIfTrainerIsNotFoundReturnFalse()
        {
            var model = new CreateProgramViewModel
            {
                Id = 1,
                Name = "New Program",
                Description = "Description of New Program",
                Duration = 10,
                ImageUrl = "newprogram.jpg"
            };

            var result = await trainingProgramServiceMock.CreateProgramAsync(model, Guid.NewGuid().ToString());

            Assert.IsFalse(result);
        }

        [Test]
        public async Task AllTrainingProgramsAsyncReturnsAllTrainingPrograms()
        {
            var programs = new List<TrainingProgram>
            {
            new TrainingProgram { Id = 1, Name = "Program 1", Description = "Description 1", Duration = 10, ImageUrl = "image1.jpg" },
            new TrainingProgram { Id = 2, Name = "Program 2", Description = "Description 2", Duration = 20, ImageUrl = "image2.jpg" },
            new TrainingProgram { Id = 3, Name = "Program 3", Description = "Description 3", Duration = 30, ImageUrl = "image3.jpg" }
            };

            dbContextMock.TrainingPrograms.AddRange(programs);
            dbContextMock.SaveChanges();

            var result = await trainingProgramServiceMock.AllTrainingProgramsAsync();

            Assert.IsNotNull(result);
            Assert.That(result.Count(), Is.EqualTo(3)); 
            Assert.IsTrue(result.Any(p => p.Id == 1 && p.Name == "Program 1" && p.Description == "Description 1" && p.Duration == 10 && p.ImageUrl == "image1.jpg"));
            Assert.IsTrue(result.Any(p => p.Id == 2 && p.Name == "Program 2" && p.Description == "Description 2" && p.Duration == 20 && p.ImageUrl == "image2.jpg"));
            Assert.IsTrue(result.Any(p => p.Id == 3 && p.Name == "Program 3" && p.Description == "Description 3" && p.Duration == 30 && p.ImageUrl == "image3.jpg"));
        }
    }
}