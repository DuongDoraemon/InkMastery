using PrintManagement.Application.InterfaceServices;
using PrintManagement.Application.Payloads.Mappers;
using PrintManagement.Application.Payloads.ResponseModels.DataNotification;
using PrintManagement.Domain.Entities;
using PrintManagement.Domain.InterfaceRepositories.InterfaceBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.ImplementServices
{
    public class NotificationService : INotificationService
    {
        private readonly IBaseReposiroty<Notification> _reposiroty;
        private readonly IBaseReposiroty<User> _userRepository;
        private readonly NotificationConverter _converter;
        public NotificationService(IBaseReposiroty<Notification> reposiroty, IBaseReposiroty<User> userRepository, NotificationConverter converter)
        {
            _reposiroty = reposiroty;
            _userRepository = userRepository;
            _converter = converter;
        }

        public async Task<bool> ConfirmIsSeenNotification(Guid notificationId)
        {

            var notification = await _reposiroty.GetByIDAsync(notificationId);
            if (notification == null)
            {
                return false;
            }
            notification.IsSeen = true;
            await _reposiroty.UpdateAsync(notification);
            return true;
        }

        public async Task<IQueryable<DataResponseNotification>> GetNotificationsByUser(Guid userId)
        {
            var user = await _userRepository.GetByIDAsync(userId);
            if(user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            var notifications = await _reposiroty.GetAllAsync(record => record.UserId == userId && record.IsActive == true);
            return notifications.Select(item => _converter.EntityToDTO(item));
        }
    }
}
