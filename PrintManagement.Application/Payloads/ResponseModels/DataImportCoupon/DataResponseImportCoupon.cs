using PrintManagement.Application.Payloads.ResponseModels.DataResource;
using PrintManagement.Application.Payloads.ResponseModels.DataUser;
using PrintManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.ResponseModels.DataImportCoupon
{
    public class DataResponseImportCoupon : DataResponseBase
    {
        public decimal TotalMoney { get; set; }
        public DataResponseResourcePropertyDetail ResourcePropertyDetail { get; set; }
        public DataResponseUser Employee { get; set; }
        public string TradingCode { get; set; }
        public int Quantity { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
