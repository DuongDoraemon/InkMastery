using PrintManagement.Application.Payloads.ResponseModels.DataNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.InterfaceServices
{
    public interface INotificationService
    {
        Task<IQueryable<DataResponseNotification>> GetNotificationsByUser(Guid userId);
        Task<bool> ConfirmIsSeenNotification(Guid notificationId);
    }
}
