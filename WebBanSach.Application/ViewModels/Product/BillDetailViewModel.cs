using System;
using System.Collections.Generic;
using System.Text;

namespace WebBanSach.Application.ViewModels.Product
{
    public class BillDetailViewModel
	{
        public int Id { get; set; }

        public int BillId { set; get; }

        public int ProductId { set; get; }

        public int Quantity { set; get; }

        public decimal Price { set; get; }

        public BillViewModel Bill { set; get; }

        public ProductViewModel Product { set; get; }
    }
}
