using System;
using System.Threading;
using System.Threading.Tasks;
using Customer.Data.IRepositories;
using Customer.Domain.Commands;
using Customer.Domain.Dtos;
using Customer.Service.Dxos;
using Customer.Service.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Customer.Service.Services
{
    public class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, CustomerDto>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerDxos _customerDxos;
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public CreateCustomerHandler(ICustomerRepository customerRepository, 
                                        IMediator mediator, 
                                        ICustomerDxos customerDxos, 
                                        ILogger<GetCustomerHandler> logger)
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _customerDxos = customerDxos ?? throw new ArgumentNullException(nameof(customerDxos));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<CustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            if (await _customerRepository.EmailExistAsync(request.Email))
            {
                string msg = $"This email {nameof(request.Email)} is already existed!";
                _logger.LogError(msg);

                //throw new ArgumentException(msg);
                throw new BadRequestException(msg);
            }

            var customer = new Domain.Models.Customer(request.Name, request.Email, request.Address, request.Age, request.PhoneNumber);

            _customerRepository.Add(customer);

            if (await _customerRepository.SaveChangesAsync() == 0)
            {
                throw new Exceptions.ApplicationException("Couldn't save data");
            }

            await _mediator.Publish(new Domain.Events.CustomerCreatedEvent(customer.Id), cancellationToken);

            var customerDto = _customerDxos.MapCustomerDto(customer);
            return customerDto;
        }
    }
}