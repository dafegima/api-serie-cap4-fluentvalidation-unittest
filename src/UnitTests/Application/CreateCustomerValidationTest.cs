using Application.Customers.Create;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using FluentValidation.TestHelper;
using Infrastructure.Repositories;
using Moq;

namespace UnitTests.Application
{
    public class CreateCustomerValidationTest
    {
        
        [SetUp]
        public void Setup()
        {
            var repo = new Mock<ICustomersRepository>();
            repo.Setup(x => x.GetById("1")).ReturnsAsync(new Customer("1", "Daniel", "Giraldo", "dg@mail.com", DateTime.Now, string.Empty, string.Empty, string.Empty));
            _customersRepository = repo.Object;
        }

        private ICustomersRepository _customersRepository;

        [Test]
        public async Task Should_Have_Error_When_Identification_IsEmpty()
        {
            //ARRANGE
            CreateCustomerCommandValidator createCustomerCommandValidator = new CreateCustomerCommandValidator(new CustomersRepository());
            CreateCustomerCommand createCustomerCommand = new() { Identification = "1" };

            //ACT
            var result = await createCustomerCommandValidator.TestValidateAsync(createCustomerCommand);

            //ASSERT
            result.ShouldHaveValidationErrorFor(x=> x.Identification).Only();
        }

        [Test]
        public async Task Should_Not_Have_Error_When_Identification_Is_Not_Empty()
        {
            //ARRANGE
            CreateCustomerCommandValidator createCustomerCommandValidator = new CreateCustomerCommandValidator(new CustomersRepository());
            CreateCustomerCommand createCustomerCommand = new() { Identification = string.Empty };

            //ACT
            var result = await createCustomerCommandValidator.TestValidateAsync(createCustomerCommand);

            //ASSERT
            result.ShouldNotHaveValidationErrorFor(x => x.Identification);
        }

        [Test]
        public async Task Should_Have_Error_When_Customer_Already_Exist()
        {
            //ARRANGE
            var repo = new Mock<ICustomersRepository>();
            repo.Setup(x => x.GetById("1")).ReturnsAsync(new Customer("1", "Daniel", "Giraldo", "dg@mail.com", DateTime.Now, string.Empty, string.Empty, string.Empty));

            CreateCustomerCommandValidator createCustomerCommandValidator = new CreateCustomerCommandValidator(repo.Object);
            CreateCustomerCommand createCustomerCommand = new CreateCustomerCommandBuilder()
                .WithIdentification("1")
                .WithFirstName("Andres")
                .WithLastName("Giraldo")
                .WithBirthDate(DateTime.Now)
                .WithEmail("ag@mail.com")
                .WithAddress("C 10 # 10 - 10")
                .WithCountry("Colombia")
                .WithCity("Medellín")
                .Build();

            //ACT
            var result = await createCustomerCommandValidator.TestValidateAsync(createCustomerCommand);

            //ASSERT
            result.ShouldHaveValidationErrorFor(x=> x.Identification).WithErrorMessage("'Identification' Customer with identification 1 already exist.").Only();
        }

        [Test]
        public async Task Should_Not_Have_Error_When_Customer_Does_Not_Exist()
        {
            //ARRANGE
            var repo = new Mock<ICustomersRepository>();
            repo.Setup(x => x.GetById("1")).ReturnsAsync(new Customer("1", "Daniel", "Giraldo", "dg@mail.com", DateTime.Now, string.Empty, string.Empty, string.Empty));

            CreateCustomerCommandValidator createCustomerCommandValidator = new CreateCustomerCommandValidator(repo.Object);
            CreateCustomerCommand createCustomerCommand = new CreateCustomerCommandBuilder()
                .WithIdentification("2")
                .WithFirstName("Andres")
                .WithLastName("Giraldo")
                .WithBirthDate(DateTime.Now)
                .WithEmail("ag@mail.com")
                .WithAddress("C 10 # 10 - 10")
                .WithCountry("Colombia")
                .WithCity("Medellín")
                .Build();

            //ACT
            var result = await createCustomerCommandValidator.TestValidateAsync(createCustomerCommand);

            //ASSERT
            result.ShouldNotHaveValidationErrorFor(x => x.Identification);
        }

        [Test]
        public async Task Should_Not_Have_Error_When_Customer_Does_Not_Exist_FirstName_Diff_Daniel_Peter_LastName_NotEmpty_Email_NotEmpty_CorrectFormat_Address_City_Country_NotEmpty()
        {
            //ARRANGE
            var repo = new Mock<ICustomersRepository>();
            repo.Setup(x => x.GetById("1")).ReturnsAsync(new Customer("1", "Daniel", "Giraldo", "dg@mail.com", DateTime.Now, string.Empty, string.Empty, string.Empty));

            CreateCustomerCommandValidator createCustomerCommandValidator = new CreateCustomerCommandValidator(repo.Object);
            CreateCustomerCommand createCustomerCommand = new CreateCustomerCommandBuilder()
                .WithIdentification("2")
                .WithFirstName("Andres")
                .WithLastName("Giraldo")
                .WithBirthDate(DateTime.Now)
                .WithEmail("ag@mail.com")
                .WithAddress("C 10 # 10 - 10")
                .WithCountry("Colombia")
                .WithCity("Medellín")
                .Build();

            //ACT
            var result = await createCustomerCommandValidator.TestValidateAsync(createCustomerCommand);

            //ASSERT
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public async Task Should_Have_Error_When_Address_IsEmpty_And_City_IsNotEmpty()
        {
            //ARRANGE
            CreateCustomerCommandValidator createCustomerCommandValidator = new CreateCustomerCommandValidator(_customersRepository);
            CreateCustomerCommand createCustomerCommand = new CreateCustomerCommandBuilder()
                .WithIdentification("2")
                .WithFirstName("Daniel")
                .WithAddress(string.Empty)
                .WithCity("Medellín")
                .Build();

            //ACT
            var result = await createCustomerCommandValidator.TestValidateAsync(createCustomerCommand);

            //ASSERT
            result.ShouldHaveValidationErrorFor(x => x.City);
        }

        [Test]
        public async Task Should_Have_BadRequestException_When_Model_Is_Not_Valid()
        {
            //ARRANGE
            CreateCustomerCommandValidator createCustomerCommandValidator = new CreateCustomerCommandValidator(_customersRepository);
            CreateCustomerCommand createCustomerCommand = new CreateCustomerCommandBuilder()
                .WithIdentification("2")
                .WithFirstName("Daniel")
                .WithAddress(string.Empty)
                .WithCountry("Colombia")
                .Build();

            var result = await createCustomerCommandValidator.TestValidateAsync(createCustomerCommand);

            //ACTION - //ASSERT
            Assert.ThrowsAsync<BadRequestException>(() => createCustomerCommandValidator.TestValidateAsync(createCustomerCommand));
        }

        [Test]
        public async Task Should_Have_Error_When_Address_IsEmpty_And_Country_IsNotEmpty()
        {
            //ARRANGE
            CreateCustomerCommandValidator createCustomerCommandValidator = new CreateCustomerCommandValidator(_customersRepository);
            CreateCustomerCommand createCustomerCommand = new CreateCustomerCommandBuilder()
                .WithIdentification("2")
                .WithFirstName("Daniel")
                .WithAddress(string.Empty)
                .WithCountry("Colombia")
                .Build();

            //ACT
            var result = await createCustomerCommandValidator.TestValidateAsync(createCustomerCommand);

            //ASSERT
            result.ShouldHaveValidationErrorFor(x => x.Country);
        }

        [Test]
        public async Task Should_Have_Error_When_FirstName_Is_Daniel()
        {
            //ARRANGE
            CreateCustomerCommandValidator createCustomerCommandValidator = new CreateCustomerCommandValidator(_customersRepository);
            CreateCustomerCommand createCustomerCommand = new CreateCustomerCommandBuilder()
                .WithIdentification("2")
                .WithFirstName("Daniel")
                .Build();

            //ACT
            var result = await createCustomerCommandValidator.TestValidateAsync(createCustomerCommand);

            //ASSERT
            result.ShouldHaveValidationErrorFor(x => x.FirstName).WithErrorMessage("FirstName must be a value different from Daniel and Peter.");
        }

        [Test]
        public async Task Should_Have_Error_When_FirstName_Is_Peter()
        {
            //ARRANGE
            CreateCustomerCommandValidator createCustomerCommandValidator = new CreateCustomerCommandValidator(_customersRepository);
            CreateCustomerCommand createCustomerCommand = new CreateCustomerCommandBuilder()
                .WithIdentification("2")
                .WithFirstName("Peter")
                .Build();

            //ACT
            var result = await createCustomerCommandValidator.TestValidateAsync(createCustomerCommand);

            //ASSERT
            result.ShouldHaveValidationErrorFor(x => x.FirstName).WithErrorMessage("FirstName must be a value different from Daniel and Peter.");
        }

        [Test]
        public async Task Should_Not_Have_Error_When_FirstName_Is_Different_From_Peter_And_Daniel()
        {
            //ARRANGE
            CreateCustomerCommandValidator createCustomerCommandValidator = new CreateCustomerCommandValidator(_customersRepository);
            CreateCustomerCommand createCustomerCommand = new CreateCustomerCommandBuilder()
                .WithIdentification("2")
                .WithFirstName("John")
                .Build();

            //ACT
            var result = await createCustomerCommandValidator.TestValidateAsync(createCustomerCommand);

            //ASSERT
            result.ShouldNotHaveValidationErrorFor(x => x.FirstName);
        }

        [Test]
        public async Task Should_Not_Have_Error_When_Identification_Have_A_Value()
        {
            //ARRANGE
            CreateCustomerCommandValidator createCustomerCommandValidator = new CreateCustomerCommandValidator(_customersRepository);
            CreateCustomerCommand createCustomerCommand = new() { Identification = "2" };

            //ACT
            var result = await createCustomerCommandValidator.TestValidateAsync(createCustomerCommand);

            //ASSERT
            result.ShouldNotHaveValidationErrorFor(x => x.Identification);
        }
    }
}