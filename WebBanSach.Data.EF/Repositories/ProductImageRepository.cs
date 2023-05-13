using System;
using System.Collections.Generic;
using System.Text;
using WebBanSach.Data.Entities;
using WebBanSach.Data.IRepositories;

namespace WebBanSach.Data.EF.Repositories
{
    public class ProductImageRepository : EFRepository<ProductImage, int>, IProductImageRepository
    {
        public ProductImageRepository(AppDbContext context) : base(context)
        {
        }
    }
}
