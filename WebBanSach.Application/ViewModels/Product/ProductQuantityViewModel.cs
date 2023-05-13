using System;
using System.Collections.Generic;
using System.Text;

namespace WebBanSach.Application.ViewModels.Product
{
    public class ProductQuantityViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public ProductViewModel Product { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime DateModified { get; set; }
	}
}
