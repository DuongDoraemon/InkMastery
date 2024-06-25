using PrintManagement.Application.Payloads.ResponseModels.DataCustomerFeedback;
using PrintManagement.Domain.Entities;
using PrintManagement.Domain.InterfaceRepositories.InterfaceBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.Mappers
{
    public class CustomerFeedbackConverter
    {
        private readonly IBaseReposiroty<User> _baseUserRepository;
        private readonly IBaseReposiroty<Customer> _baseCustomerRepository;
        private readonly UserConverter _userConverter;
        private readonly CustomerConverter _customerConverter;
        public CustomerFeedbackConverter(IBaseReposiroty<User> baseUserRepository, IBaseReposiroty<Customer> baseCustomerRepository, UserConverter userConverter, CustomerConverter customerConverter)
        {
            _baseUserRepository = baseUserRepository;
            _baseCustomerRepository = baseCustomerRepository;
            _userConverter = userConverter;
            _customerConverter = customerConverter;
        }
        public DataResponseCustomerFeedback EntityToDTOForCustomerFeedback(CustomerFeedback customerFeedback)
        {
            return new DataResponseCustomerFeedback
            {
                FeedbackContent = customerFeedback.FeedbackContent,
                FeedbackTime = customerFeedback.FeedbackTime,
                Id = customerFeedback.Id,
                ResponseByCompany = customerFeedback.ResponseByCompany,
                ResponseTime = customerFeedback.ResponseTime,
                UserFeedback = _userConverter.EntityToDTOForUser(_baseUserRepository.GetByIDAsync(customerFeedback.UserFeedbackId).Result)
            };
        }
    }
}
