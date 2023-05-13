using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WebBanSach.Data.Enums;
using WebBanSach.Data.Interfaces;
using WebBanSach.Infrastructure.SharesKernel;

namespace WebBanSach.Data.Entities
{
	[Table("ProductCategories")]
	public class ProductCategory : DomainEntity<int>, IHasSeoMetaData, ISwitchable, ISortable, IDateTracking
	{
		public ProductCategory()
		{
			Products = new List<Product>();
		}

		public ProductCategory(string name, string description, int? parentId, int? homeOrder, 
			string image, bool? homeFlag, int sortOrder, Status status,
			string seoPageTitle, string seoAlias, string seoKeywords, 
			string seoDescription)
		{
			Name = name;
			Description = description;
			ParentId = parentId;
			HomeOrder = homeOrder;
			Image = image;
			HomeFlag = homeFlag;
			SortOrder = sortOrder;
			Status = status;
			SeoPageTitle = seoPageTitle;
			SeoAlias = seoAlias;
			SeoDescription = seoDescription;
			SeoKeywords = seoKeywords;
		}

		public string Name { get; set; }

		public string Description { get; set; }

		public int? HomeOrder { get; set; }

		public int? ParentId { get; set; }

		public string Image { get; set; }

		public bool? HomeFlag { get; set; }

		public int SortOrder { get; set; }
		public string SeoPageTitle { get; set; }

		[Column(TypeName = "varchar(255)")]
		[StringLength(255)]
		public string SeoAlias { get; set; }

		[StringLength(255)]
		public string SeoDescription { get; set; }

		[StringLength(255)]
		public string SeoKeywords { get; set; }
		public Status Status { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime DateModified { get; set; }

		public virtual ICollection<Product> Products { get; set; }
	}
}
