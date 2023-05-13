using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanSach.Application.ViewModels.Blog;
using WebBanSach.Application.ViewModels.Product;
using WebBanSach.Data.Entities;
using WebBanSach.Utilities.Dtos;

namespace WebBanSach.Application.Interfaces
{
	public interface IProductQuantityService : IDisposable
	{
		List<ProductQuantityViewModel> GetAll();

		PagedResult<ProductQuantityViewModel> GetAllPaging(string startDate, string endDate, string keyword
            , int pageIndex, int pageSize);

		ProductQuantityViewModel GetById(int id);

        List<ProductQuantityViewModel> GetImport(string publisher, string startDate, string endDate);

        void SaveChanges();

		void Update(int productId, int quantity);
	}
}
