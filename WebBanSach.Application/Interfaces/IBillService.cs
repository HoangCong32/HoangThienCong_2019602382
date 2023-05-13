using System;
using System.Collections.Generic;
using System.Text;
using WebBanSach.Application.ViewModels.Product;
using WebBanSach.Data.Enums;
using WebBanSach.Utilities.Dtos;

namespace WebBanSach.Application.Interfaces
{
    public interface IBillService
    {
        void Create(BillViewModel billVm);
        void Update(BillViewModel billVm);

        PagedResult<BillViewModel> GetAllPaging(string startDate, string endDate, string keyword,
            int pageIndex, int pageSize);

        BillViewModel GetDetail(int billId);

        BillDetailViewModel CreateDetail(BillDetailViewModel billDetailVm);

        //void DeleteDetail(int productId, int billId, int colorId, int sizeId);
        void DeleteDetail(int productId, int billId);

        void UpdateStatus(int orderId, BillStatus status);

        List<BillDetailViewModel> GetBillDetails(int billId);

        //List<ColorViewModel> GetColors();

        //List<SizeViewModel> GetSizes();

        void Save();
    }
}
