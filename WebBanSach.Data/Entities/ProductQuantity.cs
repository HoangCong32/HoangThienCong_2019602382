using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WebBanSach.Data.Interfaces;
using WebBanSach.Infrastructure.SharesKernel;

namespace WebBanSach.Data.Entities
{
    [Table("ProductQuantities")]
    public class ProductQuantity : DomainEntity<int>, IDateTracking
    {
		public ProductQuantity()
		{
		}

        public ProductQuantity(int id, int productId, int quantity, DateTime dateCreated, DateTime dateModified)
        {
            Id = id;
            ProductId = productId;
            Quantity = quantity;
            DateCreated = dateCreated;
            DateModified = dateModified;
        }

        public ProductQuantity(int productId, int quantity, DateTime dateCreated, DateTime dateModified)
		{
			ProductId = productId;
			Quantity = quantity;
			DateCreated = dateCreated;
			DateModified = dateModified;
		}

        [Column(Order = 1)]
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime DateModified { get; set; }
	}
}
