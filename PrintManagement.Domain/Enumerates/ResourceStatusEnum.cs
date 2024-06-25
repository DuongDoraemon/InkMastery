using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Domain.Enumerates
{
    public enum ResourceStatusEnum
    {
        ReadyToUse = 0, // Sẵn sàng sử dụng
        NeedMaintenance = 1, // Cần bảo trì
        Broken = 2, // Đã hỏng
        OutOfStock = 3
    }
}
