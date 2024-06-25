using PrintManagement.Application.Payloads.ResponseModels.DataNotification;
using PrintManagement.Domain.Entities;
using PrintManagement.Domain.InterfaceRepositories.InterfaceBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.Mappers
{
    public class NotificationConverter
    {
        public DataResponseNotification EntityToDTO(Notification notification)
        {
            return new DataResponseNotification
            {
                Content = notification.Content,
                Id = notification.Id,
                IsSeen = notification.IsSeen,
                Link = notification.Link,
            };
        }
    }
}
