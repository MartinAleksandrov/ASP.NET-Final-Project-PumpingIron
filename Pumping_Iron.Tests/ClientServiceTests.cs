namespace Pumping_Iron.Tests
{
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Pumping_Iron.Data.Data;
    using Pumping_Iron.Services.Interfaces;
    using Pumping_Iron.Services;
    using Pumping_Iron.Data.Models;

    public class ClientServiceTests
    {
        private PumpingIronDbContext dbContextMock;
        private Mock<ITrainerService> trainerServiceMock;
        private DbContextOptions<PumpingIronDbContext> options;
        private IClientService clientServiceMock;


        [SetUp]
        public void Setup()
        {
            options = new DbContextOptionsBuilder<PumpingIronDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            dbContextMock = new PumpingIronDbContext(options);

            trainerServiceMock = new Mock<ITrainerService>();

            clientServiceMock = new ClientService(dbContextMock, trainerServiceMock.Object);
        }

        [Test]
        public async Task RemoveMyTrainerIfClientExistsWithTrainerIdTrainerClientRemoved()
        {
            var clientId = Guid.NewGuid();
            var trainerId = Guid.NewGuid();

            var client = new Client
            {
                ClientId = clientId,
                TrainerId = trainerId,
                ImageUrl = "www.pic.com"
            };

            dbContextMock.Clients.Add(client);
            await dbContextMock.SaveChangesAsync();

            trainerServiceMock.Setup(x => x.RemoveClient(clientId.ToString(), trainerId.ToString())).ReturnsAsync(true);

            var result = await clientServiceMock.RemoveMyTrainer(clientId.ToString());

            Assert.IsTrue(result);
            Assert.IsNull(await dbContextMock.Clients.FindAsync(clientId));
        }

        [Test]
        public async Task RemoveMyTrainerIfClientExistsWithoutTrainerIdReturnsFalse()
        {
            var clientId = Guid.NewGuid();

            var client = new Client
            {
                ClientId = clientId,
                TrainerId = null
            };

            dbContextMock.Clients.Add(client);
            await dbContextMock.SaveChangesAsync();

            var result = await clientServiceMock.RemoveMyTrainer(clientId.ToString());

            Assert.IsFalse(result);
            Assert.IsNotNull(await dbContextMock.Clients.FindAsync(clientId));
        }

        [Test]
        public async Task RemoveMyTrainerIfClientDoesNotExistReturnsFalse()
        {
            var clientId = Guid.NewGuid();

            var result = await clientServiceMock.RemoveMyTrainer(clientId.ToString());

            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetMyTrainerInfoIfUserExistsWithTrainerIdReturnsTrainerInfo()
        {
            var userId = Guid.NewGuid();
            var trainerId = Guid.NewGuid();

            var user = new Client
            {
                ClientId = userId,
                TrainerId = trainerId,
                ImageUrl = "www.pic.com"
            };

            dbContextMock.Clients.Add(user);
            await dbContextMock.SaveChangesAsync();

            var trainer = new Trainer
            {
                TrainerId = trainerId,
                Name = "GOSHO",
                ImageUrl = "www.pic.com",
                Information = "Some Info"
            };

            dbContextMock.Trainers.Add(trainer);
            await dbContextMock.SaveChangesAsync();

            var result = await clientServiceMock.GetMyTrainerInfo(userId.ToString());

            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(trainerId.ToString()));
            Assert.That(result.Name, Is.EqualTo(trainer.Name));
            Assert.That(result.ImageUrl, Is.EqualTo(trainer.ImageUrl));
            Assert.That(result.Information, Is.EqualTo(trainer.Information));
        }

        [Test]
        public async Task GetMyTrainerInfoIfUserExistsWithoutTrainerIdReturnsNull()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var user = new Client
            {
                ClientId = userId,
                TrainerId = null,
                ImageUrl = "www.pic.com"
            };

            dbContextMock.Clients.Add(user);
            await dbContextMock.SaveChangesAsync();

            var result = await clientServiceMock.GetMyTrainerInfo(userId.ToString());

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetMyTrainerInfoIfUserDoesNotExistReturnsNull()
        {
            var someId = Guid.NewGuid();

            var result = await clientServiceMock.GetMyTrainerInfo(someId.ToString());

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetMyProgramInfoIfUserExistsWithProgramIdReturnsProgramInfo()
        {
            var users = new[]
            {
            new Client { ClientId = Guid.NewGuid(), TrainingProgramId = 1 },
            new Client { ClientId = Guid.NewGuid(), TrainingProgramId = 0 }
            };

            var programs = new[]
            {
            new TrainingProgram { Id = 1, Name = "Program", Description = "Description", ImageUrl = "www.pic.com", Duration = 10 }
            };

            dbContextMock.Clients.AddRange(users);
            dbContextMock.TrainingPrograms.AddRange(programs);
            dbContextMock.SaveChanges();

            var userId = users[0].ClientId;

            var result = await clientServiceMock.GetMyProgramInfo(userId.ToString());

            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Name, Is.EqualTo("Program"));
            Assert.That(result.Description, Is.EqualTo("Description"));
            Assert.That(result.ImageUrl, Is.EqualTo("www.pic.com"));
            Assert.That(result.Duration, Is.EqualTo(10));
        }

        [Test]
        public async Task GetMyProgramInfoIfUserExistsWithoutProgramIdReturnsNull()
        {
            var userId = Guid.NewGuid();

            var result = await clientServiceMock.GetMyProgramInfo(userId.ToString());

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetMyProgramInfoIfUserDoesNotExistReturnsNull()
        {
            var userId = Guid.NewGuid();

            var result = await clientServiceMock.GetMyProgramInfo(userId.ToString());

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetMyDietInfoIfUserExistsWithDietIdReturnsDietInfo()
        {
            var users = new[]
            {
            new Client { ClientId = Guid.NewGuid(), DietId = 1 },
            new Client { ClientId = Guid.NewGuid(), DietId = null }
            };

            var diets = new[]
            {
            new Diet { Id = 1, Name = "Diet", Description = "Description", ImageUrl = "www.pic.com" }
            };

            dbContextMock.Diets.AddRange(diets);
            dbContextMock.Clients.AddRange(users);
            dbContextMock.SaveChanges();

            var userId = users[0].ClientId;

            var result = await clientServiceMock.GetMyDietInfo(userId.ToString());

            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Name, Is.EqualTo("Diet"));
            Assert.That(result.Description, Is.EqualTo("Description"));
            Assert.That(result.ImageUrl, Is.EqualTo("www.pic.com"));
        }

        [Test]
        public async Task GetMyDietInfoIfUserExistsWithoutDietIdReturnsNull()
        {
            var userId = Guid.NewGuid();

            var result = await clientServiceMock.GetMyDietInfo(userId.ToString());

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetMyDietInfoIFUserDoesNotExistReturnsNull()
        {
            var userId = Guid.NewGuid();

            var result = await clientServiceMock.GetMyDietInfo(userId.ToString());

            Assert.IsNull(result);
        }
    }
}