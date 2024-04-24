namespace Pumping_Iron.Tests
{
    using Microsoft.EntityFrameworkCore;
    using Pumping_Iron.Data.Data;
    using Pumping_Iron.Services.Interfaces;
    using Pumping_Iron.Services;
    using Pumping_Iron.Data.Models;
    using Pumping_Iron.Data.Models.Enums;
    using Pumping_Iron.Data.ViewModels.Client;

    public class TrainerServiceTests
    {

        private PumpingIronDbContext dbContextMock;
        private DbContextOptions<PumpingIronDbContext> options;
        private ITrainerService trainerServiceMock;


        [SetUp]
        public void Setup()
        {
            options = new DbContextOptionsBuilder<PumpingIronDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            dbContextMock = new PumpingIronDbContext(options);

            trainerServiceMock = new TrainerService(dbContextMock);
        }

        [Test]
        public async Task RemoveDietFromClientAsyncIfClientExistsWithDietIdDietRemovedFromClient()
        {
            var client1 = new Client
            {
                ClientId = Guid.NewGuid(),
                DietId = 1,
                ImageUrl = "www.pic.com"
            };

            var diet1 = new Diet
            {
                Id = 1,
                ImageUrl = "www.pic.com"
            };

            dbContextMock.Clients.Add(client1);
            dbContextMock.Diets.Add(diet1);
            dbContextMock.SaveChanges();

            var dietId = 1;
            var clientId = client1.ClientId;

            var result = await trainerServiceMock.RemoveDietFromClientAsync(dietId, clientId.ToString());

            Assert.IsTrue(result);

            var client = await dbContextMock.Clients.FirstOrDefaultAsync(c => c.ClientId == clientId);
            Assert.IsNotNull(client);
            Assert.IsNull(client.DietId);

            var diet = await dbContextMock.Diets.FirstOrDefaultAsync(d => d.Id == dietId);
            Assert.IsNotNull(diet);
            Assert.IsFalse(diet.Clients.Contains(client));
        }

        [Test]
        public async Task RemoveDietFromClientAsyncIfClientDoesNotExistReturnsFalse()
        {
            var dietId = 1;
            var clientId = Guid.NewGuid();

            var result = await trainerServiceMock.RemoveDietFromClientAsync(dietId, clientId.ToString());

            Assert.IsFalse(result);
        }

        [Test]
        public async Task RemoveDietFromClientAsyncIfDietDoesNotExistReturnsFalse()
        {
            var dietId = 856;
            var clientId = Guid.NewGuid();

            var result = await trainerServiceMock.RemoveDietFromClientAsync(dietId, clientId.ToString());

            Assert.IsFalse(result);
        }

        [Test]
        public async Task RemoveProgramFromClientAsyncIfClientExistsWithProgramIdRemovedFromClient()
        {
            var client1 = new Client
            {
                ClientId = Guid.NewGuid(),
                TrainingProgramId = 1
            };

            var trainingProgram = new TrainingProgram
            {
                Id = 1,
                ImageUrl = "www.pic.com"
            };

            dbContextMock.Clients.Add(client1);
            dbContextMock.TrainingPrograms.Add(trainingProgram);
            dbContextMock.SaveChanges();

            var programId = 1;
            var clientId = client1.ClientId;

            var result = await trainerServiceMock.RemoveProgramFromClientAsync(programId, clientId.ToString());

            Assert.IsTrue(result);

            var client = await dbContextMock.Clients.FirstOrDefaultAsync(c => c.ClientId == clientId);
            Assert.IsNotNull(client);
            Assert.IsNull(client.TrainingProgramId);

            var program = await dbContextMock.TrainingPrograms.FirstOrDefaultAsync(tp => tp.Id == programId);
            Assert.IsNotNull(program);
            Assert.IsFalse(program.Clients.Contains(client));
        }

        [Test]
        public async Task RemoveProgramFromClientAsyncIfClientDoesNotExistReturnsFalse()
        {
            var programId = 1;
            var clientId = Guid.NewGuid();

            var result = await trainerServiceMock.RemoveProgramFromClientAsync(programId, clientId.ToString());

            Assert.IsFalse(result);
        }

        [Test]
        public async Task RemoveProgramFromClientAsyncIfProgramDoesNotExistReturnsFalse()
        {
            var programId = 564;
            var clientId = Guid.NewGuid();

            var result = await trainerServiceMock.RemoveProgramFromClientAsync(programId, clientId.ToString());

            Assert.IsFalse(result);
        }

        [Test]
        public async Task AddDietToClientIFClientAndDietExistDietAddedToClient()
        {
            var diet = new Diet
            {
                Id = 1,
                ImageUrl = "www.pic.com"
            };
            dbContextMock.Diets.Add(diet);
            dbContextMock.SaveChanges();

            var dietId = 1;
            var clientId = Guid.NewGuid();

            var client = new Client
            {
                ClientId = clientId,
                ImageUrl = "www.pic.com"
            };

            dbContextMock.Clients.Add(client);
            await dbContextMock.SaveChangesAsync();

            var result = await trainerServiceMock.AddDietToClient(dietId, clientId.ToString());

            Assert.IsTrue(result);

            var updatedClient = await dbContextMock.Clients.FirstOrDefaultAsync(c => c.ClientId == clientId);
            Assert.IsNotNull(updatedClient);
            Assert.That(updatedClient.DietId, Is.EqualTo(dietId));

            var updatedDiet = await dbContextMock.Diets.FirstOrDefaultAsync(d => d.Id == dietId);
            Assert.IsNotNull(updatedDiet);
            Assert.IsTrue(updatedDiet.Clients.Contains(client));
        }

        [Test]
        public async Task AddDietToClientIfClientDoesNotExistReturnsFalse()
        {
            var dietId = 1;
            var clientId = Guid.NewGuid();

            var result = await trainerServiceMock.AddDietToClient(dietId, clientId.ToString());

            Assert.IsFalse(result);
        }

        [Test]
        public async Task AddDietToClientIfDietDoesNotExistReturnsFalse()
        {
            var dietId = 457;
            var clientId = Guid.NewGuid();

            var result = await trainerServiceMock.AddDietToClient(dietId, clientId.ToString());

            Assert.IsFalse(result);
        }

        [Test]
        public async Task AddDietToClientIfClientAlreadyHasDietReturnsFalse()
        {
            var dietId = 1;
            var clientId = Guid.NewGuid();

            var client = new Client
            {
                ClientId = clientId,
                DietId = dietId
            };

            dbContextMock.Clients.Add(client);
            await dbContextMock.SaveChangesAsync();

            var result = await trainerServiceMock.AddDietToClient(dietId, clientId.ToString());

            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetAllTrainerDietsIfTrainerExistsReturnsDiets()
        {
            var diet = new Diet
            {
                Id = 1,
                Name = "Diet",
                Description = "Description",
                ImageUrl = "www.pic.com"
            };

            var diet2 = new Diet
            {
                Id = 2,
                Name = "Diet1",
                Description = "Descriptio1n",
                ImageUrl = "www.pic1.com"
            };

            var trainer = new Trainer
            {
                TrainerId = Guid.NewGuid(),
                Diets = new List<Diet> { diet, diet2 },
                ImageUrl = "www.pic2.com"
            };

            dbContextMock.Trainers.Add(trainer);
            dbContextMock.SaveChanges();

            var trainerId = trainer.TrainerId;
            var clientId = Guid.NewGuid();

            var result = await trainerServiceMock.GetAllTrainerDiets(trainerId.ToString(), clientId.ToString());

            Assert.IsNotNull(result);
            Assert.That(result.Count(), Is.EqualTo(2));

            var diet1 = result.FirstOrDefault(d => d.Id == 1);

            Assert.IsNotNull(diet1);
            Assert.That(diet1.Name, Is.EqualTo("Diet"));
            Assert.That(diet1.Description, Is.EqualTo("Description"));
            Assert.That(diet1.ImageUrl, Is.EqualTo("www.pic.com"));
            Assert.That(diet1.ClientId, Is.EqualTo(clientId.ToString()));

        }

        [Test]
        public async Task GetAllTrainerDietsIfTrainerDoesNotExistReturnsNull()
        {
            var trainerId = Guid.NewGuid();
            var clientId = Guid.NewGuid();

            var result = await trainerServiceMock.GetAllTrainerDiets(trainerId.ToString(), clientId.ToString());

            Assert.IsNull(result);
        }

        [Test]
        public async Task AddProgramToClientIfClientAndProgramExistProgramAddedToClient()
        {
            var program = new TrainingProgram
            {
                Id = 1,
                ImageUrl = "www.pic.com"
            };

            dbContextMock.TrainingPrograms.Add(program);
            dbContextMock.SaveChanges();

            var programId = program.Id;
            var clientId = Guid.NewGuid();

            var client = new Client
            {
                ClientId = clientId,
                ImageUrl = "www.pic.com"
            };

            dbContextMock.Clients.Add(client);
            await dbContextMock.SaveChangesAsync();

            var result = await trainerServiceMock.AddProgramToClient(programId, clientId.ToString());


            Assert.IsTrue(result);

            var updatedClient = await dbContextMock.Clients.FirstOrDefaultAsync(c => c.ClientId == clientId);
            Assert.IsNotNull(updatedClient);
            Assert.That(updatedClient.TrainingProgramId, Is.EqualTo(programId));

            var updatedProgram = await dbContextMock.TrainingPrograms.FirstOrDefaultAsync(tp => tp.Id == programId);
            Assert.IsNotNull(updatedProgram);
            Assert.IsTrue(updatedProgram.Clients.Contains(client));
        }

        [Test]
        public async Task AddProgramToClientIfClientDoesNotExistReturnsFalse()
        {
            var programId = 1;
            var clientId = Guid.NewGuid();

            var result = await trainerServiceMock.AddProgramToClient(programId, clientId.ToString());

            Assert.IsFalse(result);
        }

        [Test]
        public async Task AddProgramToClientIFProgramDoesNotExistReturnsFalse()
        {
            var programId = 99;
            var clientId = Guid.NewGuid();


            var result = await trainerServiceMock.AddProgramToClient(programId, clientId.ToString());

            Assert.IsFalse(result);
        }

        [Test]
        public async Task AddProgramToClientIfClientAlreadyHasProgramReturnsFalse()
        {
            var programId = 1;
            var clientId = Guid.NewGuid();

            var client = new Client
            {
                ClientId = clientId,
                TrainingProgramId = programId
            };

            dbContextMock.Clients.Add(client);
            await dbContextMock.SaveChangesAsync();

            var result = await trainerServiceMock.AddProgramToClient(programId, clientId.ToString());

            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetAllTrainerProgramsIfTrainerExistsReturnsPrograms()
        {
            var trainerId = Guid.NewGuid();
            var program = new TrainingProgram
            {
                Id = 1,
                Name = "Program",
                Description = "Description",
                Duration = 5,
                ImageUrl = "www.pic.com"
            };

            var program2 = new TrainingProgram
            {
                Id = 2,
                Name = "Program1",
                Description = "Description1",
                Duration = 7,
                ImageUrl = "www.pic.com1"
            };

            var trainer = new Trainer
            {
                TrainerId = trainerId,
                TrainingPrograms = new List<TrainingProgram> { program, program2 },
                ImageUrl = "www.pic.com1"
            };

            dbContextMock.Trainers.Add(trainer);
            dbContextMock.SaveChanges();

            var clientId = Guid.NewGuid();

            var result = await trainerServiceMock.GetAllTrainerPrograms(trainerId.ToString(), clientId.ToString());

            Assert.IsNotNull(result);
            Assert.That(result.Count(), Is.EqualTo(2));

            var program1 = result.FirstOrDefault(p => p.Id == 1);

            Assert.IsNotNull(program1);
            Assert.That(program1.Name, Is.EqualTo("Program"));
            Assert.That(program1.Description, Is.EqualTo("Description"));
            Assert.That(program1.Duration, Is.EqualTo(5));
            Assert.That(program1.ImageUrl, Is.EqualTo("www.pic.com"));
            Assert.That(program1.ClientId, Is.EqualTo(clientId.ToString()));

            var _program = result.FirstOrDefault(p => p.Id == 2);

            Assert.IsNotNull(_program);
            Assert.That(_program.Name, Is.EqualTo("Program1"));
            Assert.That(_program.Description, Is.EqualTo("Description1"));
            Assert.That(_program.Duration, Is.EqualTo(7));
            Assert.That(_program.ImageUrl, Is.EqualTo("www.pic.com1"));
            Assert.That(_program.ClientId, Is.EqualTo(clientId.ToString()));
        }

        [Test]
        public async Task GetAllTrainerProgramsIfTrainerDoesNotExistReturnsNull()
        {
            var trainerId = Guid.NewGuid();
            var clientId = Guid.NewGuid();

            var result = await trainerServiceMock.GetAllTrainerPrograms(trainerId.ToString(), clientId.ToString());

            Assert.IsNull(result);
        }

        [Test]
        public async Task IsClientAlreadyHireTrainerAsyncIfClientExistsAndNotHiredReturnsTrue()
        {
            var clientId = Guid.NewGuid();

            var client = new Client
            {
                ClientId = clientId,
                ImageUrl = "www.pic.com"
            };

            dbContextMock.Clients.Add(client);
            await dbContextMock.SaveChangesAsync();

            var result = await trainerServiceMock.IsClientAlreadyHireTrainerAsync(clientId.ToString());

            Assert.IsTrue(result);
        }

        [Test]
        public async Task IsClientAlreadyHireTrainerAsyncIfClientExistsAndHiredReturnsFalse()
        {
            var clientId = Guid.NewGuid();
            var trainerId = Guid.NewGuid();

            var trainer = new Trainer()
            {
                TrainerId = trainerId,
                ImageUrl = "www.pic.com"
            };

            var client = new Client
            {
                ClientId = clientId,
                TrainerId = trainerId,
                Trainer = trainer,
                ImageUrl = "www.pic.com"
            };
            dbContextMock.Clients.Add(client);
            await dbContextMock.SaveChangesAsync();

            var result = await trainerServiceMock.IsClientAlreadyHireTrainerAsync(client.ClientId.ToString());

            Assert.IsFalse(result);
        }

        [Test]
        public async Task IsClientAlreadyHireTrainerAsyncIfClientDoesNotExistReturnsFalse()
        {
            var clientId = Guid.NewGuid();

            var result = await trainerServiceMock.IsClientAlreadyHireTrainerAsync(clientId.ToString());

            Assert.IsFalse(result);
        }

        [Test]
        public async Task AllTrainersAsyncIfReturnsAllTrainers()
        {
            var trainer1 = new Trainer
            {
                TrainerId = Guid.NewGuid(),
                Name = "Trainer 1",
                Age = 30,
                Gender = Gender.Male,
                ImageUrl = "www.pic.com"
            };

            var trainer2 = new Trainer
            {
                TrainerId = Guid.NewGuid(),
                Name = "Trainer 2",
                Age = 35,
                Gender = Gender.Female,
                ImageUrl = "www.pic.com"
            };

            dbContextMock.Trainers.AddRange(trainer1, trainer2);
            dbContextMock.SaveChanges();

            var result = await trainerServiceMock.AllTrainersAsync();

            Assert.IsNotNull(result);
            Assert.That(result.Count(), Is.EqualTo(2));

            var trainer = result.FirstOrDefault(t => t.Name == "Trainer 1");

            Assert.IsNotNull(trainer1);
            Assert.That(trainer1.Age, Is.EqualTo(30));
            Assert.That(trainer1.Gender, Is.EqualTo(Gender.Male));
            Assert.That(trainer1.ImageUrl, Is.EqualTo("www.pic.com"));

            var secondTrainer = result.FirstOrDefault(t => t.Name == "Trainer 2");

            Assert.IsNotNull(trainer2);
            Assert.That(secondTrainer!.Age, Is.EqualTo(35));
            Assert.That(secondTrainer.Gender, Is.EqualTo(Gender.Female.ToString()));
            Assert.That(secondTrainer.ImageUrl, Is.EqualTo("www.pic.com"));
        }

        [Test]
        public async Task GetTrainerDetailsAsyncIfTrainerExists_ReturnsTrainerDetails()
        {
            var trainerId = Guid.NewGuid();

            var trainer = new Trainer
            {
                TrainerId = trainerId,
                Name = "Trainer 1",
                Information = "Information about Trainer 1",
                ImageUrl = "www.pic.com" 
            };
            dbContextMock.Trainers.Add(trainer);
            dbContextMock.SaveChanges();

            var _trainerId = dbContextMock.Trainers.First().TrainerId.ToString();

            var result = await trainerServiceMock.GetTrainerDetailsAsync(_trainerId);

            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(trainerId.ToString()));
            Assert.That(result.Name, Is.EqualTo("Trainer 1"));
            Assert.That(result.Information, Is.EqualTo("Information about Trainer 1"));
            Assert.That(result.ImageUrl, Is.EqualTo("www.pic.com"));
        }

        [Test]
        public async Task GetTrainerDetailsAsyncIfTrainerDoesNotExistReturnsNull()
        {
            var trainerId = Guid.NewGuid();

            var result = await trainerServiceMock.GetTrainerDetailsAsync(trainerId.ToString());

            Assert.IsNull(result);
        }

        [Test]
        public async Task HireTrainerAsyncClientDoesNotExistAndNotHiredBeforeSuccessfullyHiresTrainer()
        {
            var trainerId = Guid.NewGuid();
            var clientId = Guid.NewGuid();

            var model = new HireTrainerViewModel
            {
                Name = "Client 1",
                Age = 25,
                ImageUrl = "img.com",
                Gender = Gender.Male,
                TrainerId = trainerId,
            };

            var result = await trainerServiceMock.HireTrainerAsync(model, trainerId.ToString(), clientId.ToString());

            Assert.IsFalse(result);

            var hiredClient = await dbContextMock.Clients.FirstOrDefaultAsync(c => c.ClientId == clientId);
            
            Assert.IsNull(hiredClient);
        }

        [Test]
        public async Task HireTrainerAsyncIfClientAlreadyExistsReturnsFalse()
        {
            var clientId = Guid.NewGuid();
            var trainerId = Guid.NewGuid();

            var existingClient = new Client 
            { 
                ClientId = clientId,
                ImageUrl = "www.pic.com"
            };
            dbContextMock.Clients.Add(existingClient);
            await dbContextMock.SaveChangesAsync();
            var model = new HireTrainerViewModel();

            var result = await trainerServiceMock.HireTrainerAsync(model, trainerId.ToString(), clientId.ToString());

            Assert.IsFalse(result);
        }

        [Test]
        public async Task HireTrainerAsync_ClientAlreadyHired_ReturnsFalse()
        {
            var clientId = Guid.NewGuid().ToString();
            var trainerId = Guid.NewGuid().ToString();

            var existingClient = new Client
            {
                ClientId = Guid.Parse(clientId),
                TrainerId = Guid.Parse(trainerId),
                ImageUrl = "www.pic.com"
            };

            dbContextMock.Clients.Add(existingClient);
            await dbContextMock.SaveChangesAsync();

            var model = new HireTrainerViewModel();

            var result = await trainerServiceMock.HireTrainerAsync(model, trainerId, clientId);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task HireTrainerAsyncIfTrainerNotFoundReturnsFalse()
        {
            var trainerId = Guid.NewGuid().ToString();
            var clientId = Guid.NewGuid().ToString();
            var model = new HireTrainerViewModel();

            var result = await trainerServiceMock.HireTrainerAsync(model, trainerId, clientId);

            Assert.IsFalse(result);
        }
    }
}