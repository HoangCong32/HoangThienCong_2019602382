using System.Collections.Generic;
using WebBanSach.Application.ViewModels.Common;
using WebBanSach.Application.ViewModels.Product;

namespace WebBanSach.Models
{
	public class HomeViewModel
	{
        public List<SlideViewModel> HomeSlides { get; set; }
        public List<ProductViewModel> HotProducts { get; set; }
        public List<ProductViewModel> TopSellProducts { get; set; }

        public List<ProductCategoryViewModel> HomeCategories { set; get; }

        public string Title { set; get; }
        public string MetaKeyword { set; get; }
        public string MetaDescription { set; get; }
    }
}
