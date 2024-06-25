using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Domain.Enumerates
{
    public enum PrintJobStatusEnum
    {
        Pending = 0,
        Printing = 1,
        Paused = 2,
        Completed = 3
    }
}
