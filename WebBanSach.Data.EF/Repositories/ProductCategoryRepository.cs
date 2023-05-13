using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using WebBanSach.Data.Entities;
using WebBanSach.Data.IRepositories;

namespace WebBanSach.Data.EF.Repositories
{
	public class ProductCategoryRepository : EFRepository<ProductCategory, int>, IProductCategoryRepository
	{
		AppDbContext _context;
		public ProductCategoryRepository(AppDbContext context) : base(context)
		{
			_context = context;
		}

		public List<ProductCategory> GetByAlias(string alias)
		{
			return _context.ProductCategories.Where(x => x.SeoAlias == alias).ToList();
		}
    }
}
