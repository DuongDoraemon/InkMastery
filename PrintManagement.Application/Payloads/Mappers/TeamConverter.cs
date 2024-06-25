using PrintManagement.Application.Payloads.ResponseModels.DataTeam;
using PrintManagement.Domain.Entities;
using PrintManagement.Domain.InterfaceRepositories.InterfaceBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.Mappers
{
    public class TeamConverter
    {
        private readonly IBaseReposiroty<User> _baseUserRepository;
        private readonly IBaseReposiroty<Team> _baseTeamRepository;
        private readonly UserConverter _userConverter;
        public TeamConverter(IBaseReposiroty<User> baseUserRepository, UserConverter userConverter, IBaseReposiroty<Team> baseTeamRepository)
        {
            _baseUserRepository = baseUserRepository;
            _userConverter = userConverter;
            _baseTeamRepository = baseTeamRepository;
        }
        public DataResponseTeam EntityToDTO(Team entity)
        {
            var teamItem = _baseTeamRepository.GetByIDAsync(entity.Id).Result;
            var user = _baseUserRepository.GetByIDAsync(entity.ManagerId).Result;
            var team = new DataResponseTeam
            {
                CreateTime = entity.CreateTime,
                Description = entity.Description,
                Id = entity.Id,
                ManagerName = user != null ? user.FullName : "",
                Name = entity.Name,
                NumberOfMember = entity.NumberOfMember,
                UpdateTime = entity.UpdateTime,
                Users = _baseUserRepository.GetAllAsync(x => x.TeamId == entity.Id).Result != null ? _baseUserRepository.GetAllAsync(x => x.TeamId == entity.Id).Result.Select(x => _userConverter.EntityToDTOForUser(x)) : null
            };
            return team;
        }
    }
}
