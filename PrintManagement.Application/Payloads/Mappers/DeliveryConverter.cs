using PrintManagement.Application.Payloads.ResponseModels.DataDelivery;
using PrintManagement.Domain.Entities;
using PrintManagement.Domain.InterfaceRepositories.InterfaceBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.Mappers
{
    public class DeliveryConverter
    {
        private readonly IBaseReposiroty<Customer> _baseCustomerRepository;
        private readonly IBaseReposiroty<User> _baseUserRepository;
        private readonly IBaseReposiroty<Project> _baseProjectRepository;
        private readonly IBaseReposiroty<ShippingMethod> _baseShippingMethodRepository;
        private readonly CustomerConverter _customerConverter;
        private readonly UserConverter _userConverter;
        private readonly ProjectConverter _projectConverter;
        private readonly IBaseReposiroty<ConfirmReceiptOfGoodsFromCustomer> _confirmRepository;
        private readonly ConfirmReceiptConverter _confirmReceiptConverter;
        public DeliveryConverter(IBaseReposiroty<Customer> baseCustomerRepository, IBaseReposiroty<User> baseUserRepository, IBaseReposiroty<Project> baseProjectRepository, IBaseReposiroty<ShippingMethod> baseShippingMethodRepository, CustomerConverter customerConverter, UserConverter userConverter, ProjectConverter projectConverter, IBaseReposiroty<ConfirmReceiptOfGoodsFromCustomer> confirmRepository, ConfirmReceiptConverter confirmReceiptConverter)
        {
            _baseCustomerRepository = baseCustomerRepository;
            _baseUserRepository = baseUserRepository;
            _baseProjectRepository = baseProjectRepository;
            _baseShippingMethodRepository = baseShippingMethodRepository;
            _customerConverter = customerConverter;
            _userConverter = userConverter;
            _projectConverter = projectConverter;
            _confirmRepository = confirmRepository;
            _confirmReceiptConverter = confirmReceiptConverter;
        }

        public DataResponseDelivery EntityToDTO(Delivery delivery)
        {
            return new DataResponseDelivery
            {
                ActualDeliveryTime = delivery.ActualDeliveryTime,
                DeliveryAddress = delivery.DeliveryAddress,
                DeliveryStatus = delivery.DeliveryStatus.ToString(),
                Id = delivery.Id,
                EstimateDeliveryTime = delivery.EstimateDeliveryTime,
                Customer = _customerConverter.EntityToDTOForCustomer(_baseCustomerRepository.GetAsync(x => x.Id == delivery.CustomerId).Result),
                Deliver = _userConverter.EntityToDTOForUser(_baseUserRepository.GetAsync(x => x.Id == delivery.DeliverId).Result),
                Project = _projectConverter.EntityToDTOForProject(_baseProjectRepository.GetAsync(x => x.Id == delivery.ProjectId).Result),
                ShippingMethodName = _baseShippingMethodRepository.GetAsync(x => x.Id == delivery.ShippingMethodId).Result.ShippingMethodName,
                ConfirmReceiptOfGoods = _confirmRepository.GetAllAsync(x => x.DeliveryId == delivery.Id).Result.Select(x => _confirmReceiptConverter.EntityToDTO(x))
            };
        }
    }
}
