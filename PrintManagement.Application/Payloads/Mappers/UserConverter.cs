using PrintManagement.Application.Payloads.ResponseModels.DataUser;
using PrintManagement.Domain.Entities;
using PrintManagement.Domain.InterfaceRepositories.InterfaceBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.Mappers
{
    public class UserConverter
    {
        private readonly IBaseReposiroty<Team> _baseTeamRepository;
        private readonly IBaseReposiroty<Notification> _notificationRepository;
        private readonly NotificationConverter _notificationConverter;
        public UserConverter(IBaseReposiroty<Team> baseTeamRepository, IBaseReposiroty<Notification> notificationRepository, NotificationConverter notificationConverter)
        {
            _baseTeamRepository = baseTeamRepository;
            _notificationRepository = notificationRepository;
            _notificationConverter = notificationConverter;
        }
        public DataResponseUser EntityToDTOForUser(User user)
        {
            return new DataResponseUser
            {
                CreateTime = user.CreateTime,
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                FullName = user.FullName,
                Avatar = user.Avatar,
                Id = user.Id,
                Gender = user.Gender.ToString(),
                PhoneNumber = user.PhoneNumber,
                TeamName = _baseTeamRepository.GetAsync(x => x.Id == user.TeamId).Result.Name,
                DataResponseNotifications = _notificationRepository.GetAllAsync(x => x.UserId == user.Id).Result.Select(x => _notificationConverter.EntityToDTO(x))
            };
        }
    }
}
