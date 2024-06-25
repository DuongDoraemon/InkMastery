using PrintManagement.Application.Payloads.ResponseModels.DataDesign;
using PrintManagement.Domain.Entities;
using PrintManagement.Domain.InterfaceRepositories.InterfaceBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.Mappers
{
    public class DesignConverter
    {
        private readonly IBaseReposiroty<User> _baseUserRepository;
        private readonly IBaseReposiroty<Project> _baseProjectRepository;
        private readonly IBaseReposiroty<Customer> _baseCustomerRepository;
        private readonly IBaseReposiroty<Design> _baseDesignRepository;
        private readonly UserConverter _userConverter;
        public DesignConverter(IBaseReposiroty<User> baseUserRepository, IBaseReposiroty<Project> baseProjectRepository, IBaseReposiroty<Customer> baseCustomerRepository, IBaseReposiroty<Design> baseDesignRepository, UserConverter userConverter)
        {
            _baseUserRepository = baseUserRepository;
            _baseProjectRepository = baseProjectRepository;
            _baseCustomerRepository = baseCustomerRepository;
            _baseDesignRepository = baseDesignRepository;
            _userConverter = userConverter;
        }
        public DataResponseDesign EntityToDTOForDesign(Design design)
        {
            var approver = _baseUserRepository.GetByIDAsync(design.ApproverId);
            var designer = _baseUserRepository.GetByIDAsync(design.DesignerId);
            return new DataResponseDesign
            {
                Approver = approver.Result != null ? approver.Result.FullName : null ,
                Designer = designer.Result.FullName,
                DesignImage = design.DesignImage,
                DesignStatus = design.DesignStatus.ToString(),
                DesignTime = design.DesignTime,
                Id = design.Id
            };
        }
    }
}
