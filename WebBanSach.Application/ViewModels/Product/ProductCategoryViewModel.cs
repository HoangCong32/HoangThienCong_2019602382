using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WebBanSach.Application.ViewModels.Blog;
using WebBanSach.Data.Enums;

namespace WebBanSach.Application.ViewModels.Product
{
	public class ProductCategoryViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public string Description { get; set; }

		public int? HomeOrder { get; set; }

		/// <summary>
		/// Kieu khoa ngoai, lien ket voi chinh no
		/// </summary>
		public int? ParentId { get; set; }

		public string Image { get; set; }

		/// <summary>
		/// Co hien thi tren home hay khong
		/// </summary>
		public bool? HomeFlag { get; set; }

		public int SortOrder { get; set; }
		public string SeoPageTitle { get; set; }
		public string SeoAlias { get; set; }
		public string SeoDescription { get; set; }
		public string SeoKeywords { get; set; }
		public Status Status { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime DateModified { get; set; }

		public ICollection<ProductViewModel> Products { get; set; }
    }
}
