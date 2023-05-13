using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WebBanSach.Application.ViewModels.Blog;
using WebBanSach.Application.ViewModels.Common;
using WebBanSach.Application.ViewModels.Product;
using WebBanSach.Application.ViewModels.System;
using WebBanSach.Data.Entities;

namespace WebBanSach.Application.AutoMapper
{
	public class DomainToViewModelMappingProfile : Profile
	{
		public DomainToViewModelMappingProfile()
		{
			CreateMap<ProductCategory, ProductCategoryViewModel>();
			CreateMap<Product, ProductViewModel>();

			CreateMap<Function, FunctionViewModel>();
			CreateMap<AppUser, AppUserViewModel>();
			CreateMap<AppRole, AppRoleViewModel>();
			CreateMap<Bill, BillViewModel>();
			CreateMap<BillDetail, BillDetailViewModel>();
			CreateMap<ProductQuantity, ProductQuantityViewModel>().MaxDepth(2);
			CreateMap<ProductImage, ProductImageViewModel>().MaxDepth(2);
			CreateMap<WholePrice, WholePriceViewModel>().MaxDepth(2);

			CreateMap<SystemConfig, SystemConfigViewModel>().MaxDepth(2);
			CreateMap<Footer, FooterViewModel>().MaxDepth(2);

			CreateMap<Feedback, FeedbackViewModel>().MaxDepth(2);
			CreateMap<Contact, ContactViewModel>().MaxDepth(2);
			CreateMap<Page, PageViewModel>().MaxDepth(2);
		}
	}
}
