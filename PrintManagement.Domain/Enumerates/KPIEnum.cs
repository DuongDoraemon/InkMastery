using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PrintManagement.Domain.Enumerates
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum KPIEnum
    {
        Month = 0,
        Quater = 1,
        Year = 2
    }
}
