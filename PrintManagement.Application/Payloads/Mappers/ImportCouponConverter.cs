using PrintManagement.Application.Payloads.ResponseModels.DataImportCoupon;
using PrintManagement.Domain.Entities;
using PrintManagement.Domain.InterfaceRepositories.InterfaceBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.Mappers
{
    public class ImportCouponConverter
    {
        private readonly IBaseReposiroty<ResourcePropertyDetail> _baseDetailRepository;
        private readonly IBaseReposiroty<User> _baseUserRepository;
        private readonly ResourcePropertyDetailConverter _detailConverter;
        private readonly UserConverter _userConverter;
        public ImportCouponConverter(IBaseReposiroty<ResourcePropertyDetail> baseDetailRepository, IBaseReposiroty<User> baseUserRepository, ResourcePropertyDetailConverter detailConverter, UserConverter userConverter)
        {
            _baseDetailRepository = baseDetailRepository;
            _baseUserRepository = baseUserRepository;
            _detailConverter = detailConverter;
            _userConverter = userConverter;
        }

        public DataResponseImportCoupon EntityToDTO(ImportCoupon entity)
        {
            return new DataResponseImportCoupon
            {
                CreateTime = entity.CreateTime,
                Id = entity.Id,
                TotalMoney = entity.TotalMoney,
                TradingCode = entity.TradingCode,
                Quantity = entity.Quantity,
                UpdateTime = entity.UpdateTime,
                Employee = _userConverter.EntityToDTOForUser(_baseUserRepository.GetByIDAsync(entity.EmployeeId).Result),
                ResourcePropertyDetail = _detailConverter.EntityToDTO(_baseDetailRepository.GetByIDAsync(entity.ResourcePropertyDetailId).Result)
            };
        }
    }
}
