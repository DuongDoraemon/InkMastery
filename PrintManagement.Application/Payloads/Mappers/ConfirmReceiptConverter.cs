using PrintManagement.Application.Payloads.ResponseModels.DataConfirmReceiptOfGoodsFromCustomer;
using PrintManagement.Domain.Entities;
using PrintManagement.Domain.InterfaceRepositories.InterfaceBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.Mappers
{
    public class ConfirmReceiptConverter
    {
        public DataResponseConfirmReceiptOfGoodsFromCustomer EntityToDTO(ConfirmReceiptOfGoodsFromCustomer entity)
        {
            return new DataResponseConfirmReceiptOfGoodsFromCustomer
            {
                Id = entity.Id,
                ConfirmReceiptOfGoods = entity.ConfirmReceiptOfGoods.ToString(),
            };
        }
    }
}
