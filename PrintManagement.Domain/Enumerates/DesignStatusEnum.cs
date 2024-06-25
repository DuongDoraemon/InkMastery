using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PrintManagement.Domain.Enumerates
{
    public enum DesignStatusEnum
    {
        NotYetApproved = 0,
        HasBeenApproved = 1,
        Refuse = 2
    }
}
