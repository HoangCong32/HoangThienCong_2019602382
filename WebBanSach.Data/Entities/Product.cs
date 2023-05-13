using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WebBanSach.Data.Enums;
using WebBanSach.Data.Interfaces;
using WebBanSach.Infrastructure.SharesKernel;

namespace WebBanSach.Data.Entities
{
    [Table("Products")]
    public class Product : DomainEntity<int>, IHasSeoMetaData, ISwitchable, ISortable, IDateTracking
    {
		public Product()
		{
			ProductTags = new List<ProductTag>();
		}

		public Product(string name, int categoryId, string author, 
            string publisher, string image, decimal price, 
            decimal? promotionPrice, decimal originalPrice, 
            string description, string content, bool? homeFlag, 
            bool? hotFlag, int? viewCount, string tags, string seoPageTitle, 
            string seoAlias, string seoDescription, 
            string seoKeywords, Status status, int sortOrder, 
            DateTime dateCreated, DateTime dateModified)
		{
			Name = name;
			CategoryId = categoryId;
			Author = author;
			Publisher = publisher;
			Image = image;
			Price = price;
			PromotionPrice = promotionPrice;
			OriginalPrice = originalPrice;
			Description = description;
			Content = content;
			HomeFlag = homeFlag;
			HotFlag = hotFlag;
			ViewCount = viewCount;
			Tags = tags;
			SeoPageTitle = seoPageTitle;
			SeoAlias = seoAlias;
			SeoDescription = seoDescription;
			SeoKeywords = seoKeywords;
			Status = status;
			SortOrder = sortOrder;
			DateCreated = dateCreated;
			DateModified = dateModified;
			ProductTags = new List<ProductTag>();
		}

		public Product(int id, string name, int categoryId, string author,
			string publisher, string image, decimal price,
			decimal? promotionPrice, decimal originalPrice,
			string description, string content, bool? homeFlag,
			bool? hotFlag, int? viewCount, string tags, string seoPageTitle,
			string seoAlias, string seoDescription,
			string seoKeywords, Status status, int sortOrder,
			DateTime dateCreated, DateTime dateModified)
		{
			Id = id;
			Name = name;
			CategoryId = categoryId;
			Author = author;
			Publisher = publisher;
			Image = image;
			Price = price;
			PromotionPrice = promotionPrice;
			OriginalPrice = originalPrice;
			Description = description;
			Content = content;
			HomeFlag = homeFlag;
			HotFlag = hotFlag;
			ViewCount = viewCount;
			Tags = tags;
			SeoPageTitle = seoPageTitle;
			SeoAlias = seoAlias;
			SeoDescription = seoDescription;
			SeoKeywords = seoKeywords;
			Status = status;
			SortOrder = sortOrder;
			DateCreated = dateCreated;
			DateModified = dateModified;
			ProductTags = new List<ProductTag>();
		}

		[StringLength(255)]
        [Required]
        public string Name { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [StringLength(255)]
        public string Author { get; set; }

        [StringLength(255)]
        public string Publisher { get; set; }

        [StringLength(255)]
        public string Image { get; set; }

        [Required]
        [DefaultValue(0)]
        public decimal Price { get; set; }

        public decimal? PromotionPrice { get; set; }

        [Required]
        public decimal OriginalPrice { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public string Content { get; set; }

        public bool? HomeFlag { get; set; }

        public bool? HotFlag { get; set; }

        public int? ViewCount { get; set; }

        [StringLength(255)]
        public string Tags { get; set; }

        [ForeignKey("CategoryId")]
        public virtual ProductCategory ProductCategory { set; get; }

		public virtual ICollection<ProductTag> ProductTags { set; get; }

		public string SeoPageTitle { get; set; }

        [Column(TypeName = "varchar(255)")]
        [StringLength(255)]
        public string SeoAlias { get; set; }

        [StringLength(255)]
        public string SeoDescription { get; set; }

        [StringLength(255)]
        public string SeoKeywords { get; set; }
        public Status Status { get; set; }
		public int SortOrder { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime DateModified { get; set; }
	}
}
