using PrintManagement.Application.Payloads.ResponseModels.DataCustomer;
using PrintManagement.Domain.Entities;
using PrintManagement.Domain.InterfaceRepositories.InterfaceBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.Mappers
{
    public class CustomerConverter
    {
        private readonly IBaseReposiroty<Customer> _baseCustomerRepository;
        public CustomerConverter(IBaseReposiroty<Customer> baseCustomerRepository)
        {
            _baseCustomerRepository = baseCustomerRepository;
        }
        public DataResponseCustomer EntityToDTOForCustomer(Customer customer)
        {
            return new DataResponseCustomer
            {
                Address = customer.Address,
                FullName = customer.FullName,
                Id = customer.Id,
                PhoneNumber = customer.PhoneNumber,
                Email = customer.Email,
            };
        }
    }
}
