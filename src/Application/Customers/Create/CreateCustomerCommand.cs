using Application.Commons;
using Application.Customers.Commons;
using MediatR;

namespace Application.Customers.Create
{
	public class CreateCustomerCommand : CustomerBase, IRequest<Result<CreateCustomerCommandResponse>>
	{
        
	}

    public class CreateCustomerCommandBuilder
    {
        private CreateCustomerCommand _createCustomerCommand = new();

        public CreateCustomerCommandBuilder WithIdentification(string identification)
        {
            _createCustomerCommand.Identification = identification;
            return this;
        }

        public CreateCustomerCommandBuilder WithFirstName(string firstName)
        {
            _createCustomerCommand.FirstName = firstName;
            return this;
        }

        public CreateCustomerCommandBuilder WithLastName(string lastName)
        {
            _createCustomerCommand.LastName = lastName;
            return this;
        }

        public CreateCustomerCommandBuilder WithBirthDate(DateTime birthDate)
        {
            _createCustomerCommand.BirthDate = birthDate;
            return this;
        }

        public CreateCustomerCommandBuilder WithEmail(string email)
        {
            _createCustomerCommand.Email = email;
            return this;
        }

        public CreateCustomerCommandBuilder WithAddress(string address)
        {
            _createCustomerCommand.Address = address;
            return this;
        }

        public CreateCustomerCommandBuilder WithCity(string city)
        {
            _createCustomerCommand.City = city;
            return this;
        }

        public CreateCustomerCommandBuilder WithCountry(string country)
        {
            _createCustomerCommand.Country = country;
            return this;
        }

        public CreateCustomerCommand Build() => _createCustomerCommand;
    }
}