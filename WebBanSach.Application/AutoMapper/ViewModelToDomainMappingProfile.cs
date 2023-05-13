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
	public class ViewModelToDomainMappingProfile : Profile
	{
		public ViewModelToDomainMappingProfile()
		{
			CreateMap<ProductCategoryViewModel, ProductCategory>()
				.ConstructUsing(c => new ProductCategory(c.Name, c.Description, c.ParentId, c.HomeOrder, c.Image, c.HomeFlag,
				c.SortOrder, c.Status, c.SeoPageTitle, c.SeoAlias, c.SeoKeywords, c.SeoDescription));

			CreateMap<ProductViewModel, Product>()
		   .ConstructUsing(c => new Product(c.Name, c.CategoryId, c.Author,
			c.Publisher, c.Image, c.Price,
			c.PromotionPrice, c.OriginalPrice,
			c.Description, c.Content, c.HomeFlag,
			c.HotFlag, c.ViewCount, c.Tags, c.SeoPageTitle,
			c.SeoAlias, c.SeoDescription,
			c.SeoKeywords, c.Status, c.SortOrder,
			c.DateCreated, c.DateModified));

			CreateMap<AppUserViewModel, AppUser>()
			.ConstructUsing(c => new AppUser(c.Id.GetValueOrDefault(Guid.Empty), c.FullName, c.UserName,
			c.Email, c.PhoneNumber, c.Avatar, c.Status));

			CreateMap<PermissionViewModel, Permission>()
			.ConstructUsing(c => new Permission(c.RoleId, c.FunctionId, c.CanCreate, c.CanRead, c.CanUpdate, c.CanDelete));

			CreateMap<BillViewModel, Bill>()
			  .ConstructUsing(c => new Bill(c.Id, c.CustomerName, c.CustomerAddress,
			  c.CustomerMobile, c.CustomerMessage, c.BillStatus,
			  c.PaymentMethod, c.Status, c.CustomerId));

			CreateMap<BillDetailViewModel, BillDetail>()
			  .ConstructUsing(c => new BillDetail(c.Id, c.BillId, c.ProductId,
			  c.Quantity, c.Price));

			CreateMap<ContactViewModel, Contact>()
				.ConstructUsing(c => new Contact(c.Id, c.Name, c.Phone, c.Email, c.Website, c.Address, c.Other, c.Lng, c.Lat, c.Status));

			CreateMap<FeedbackViewModel, Feedback>()
				.ConstructUsing(c => new Feedback(c.Id, c.Name, c.Email, c.Message, c.Status));
			
			CreateMap<PageViewModel, Page>()
			 .ConstructUsing(c => new Page(c.Id, c.Name, c.Alias, c.Content, c.Status));

			CreateMap<ProductQuantityViewModel, ProductQuantity>()
				.ConstructUsing(c => new ProductQuantity(c.ProductId, c.Quantity, c.DateCreated, c.DateModified));
		}
	}
}
