using PrintManagement.Application.Payloads.ResponseModels.DataConfirmReceiptOfGoodsFromCustomer;
using PrintManagement.Application.Payloads.ResponseModels.DataCustomer;
using PrintManagement.Application.Payloads.ResponseModels.DataProject;
using PrintManagement.Application.Payloads.ResponseModels.DataUser;
using PrintManagement.Domain.Entities;
using PrintManagement.Domain.Enumerates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.ResponseModels.DataDelivery
{
    public class DataResponseDelivery : DataResponseBase
    {
        public string ShippingMethodName { get; set; }
        public DataResponseCustomer Customer { get; set; }
        public DataResponseUser Deliver { get; set; }
        public DataResponseProject Project { get; set; }
        public string DeliveryAddress { get; set; }
        public DateTime EstimateDeliveryTime { get; set; }
        public DateTime? ActualDeliveryTime { get; set; }
        public string DeliveryStatus { get; set; }
        public IQueryable<DataResponseConfirmReceiptOfGoodsFromCustomer> ConfirmReceiptOfGoods { get; set; }
    }
}
