using PrintManagement.Application.Payloads.ResponseModels.DataDelivery;
using PrintManagement.Domain.Entities;
using PrintManagement.Domain.Enumerates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.ResponseModels.DataConfirmReceiptOfGoodsFromCustomer
{
    public class DataResponseConfirmReceiptOfGoodsFromCustomer : DataResponseBase
    {
        public string ConfirmReceiptOfGoods { get; set; }
    }
}
