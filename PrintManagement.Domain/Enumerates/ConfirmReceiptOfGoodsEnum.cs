using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PrintManagement.Domain.Enumerates
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ConfirmReceiptOfGoodsEnum
    {
        NotReceived = 0,
        Reject = 1,
        Received = 2
    }
}
