using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WebBanSach.Application.Interfaces;
using WebBanSach.Application.ViewModels.Product;
using WebBanSach.Data.Entities;
using WebBanSach.Data.IRepositories;
using WebBanSach.Infrastructure.Interfaces;
using WebBanSach.Utilities.Dtos;

namespace WebBanSach.Application.Implementation
{
	public class ProductQuantityService : IProductQuantityService
	{
		private readonly IProductQuantityRepository _productQuantityRepository;
		private IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public ProductQuantityService(IProductQuantityRepository productQuantityRepository, IUnitOfWork unitOfWork, IMapper mapper)
		{
			_productQuantityRepository = productQuantityRepository;
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}

		public List<ProductQuantityViewModel> GetAll()
		{
			return _mapper.ProjectTo<ProductQuantityViewModel>(_productQuantityRepository.FindAll()).ToList();
		}

        public ProductQuantityViewModel GetById(int id)
		{
			return _mapper.Map<ProductQuantity, ProductQuantityViewModel>(_productQuantityRepository.FindById(id));
		}


		public void SaveChanges()
		{
			_unitOfWork.Commit();
		}

        public void Update(int productId, int quantity)
        {
            var pro = _productQuantityRepository.FindSingle(x => x.ProductId == productId && x.Quantity > 0);
            pro.Quantity = quantity;
            _productQuantityRepository.Update(pro);
        }

        PagedResult<ProductQuantityViewModel> IProductQuantityService.GetAllPaging(string startDate, string endDate, string keyword, int pageIndex, int pageSize)
        {
            var query = _productQuantityRepository.FindAll();
            if (!string.IsNullOrEmpty(startDate))
            {
                //DateTime start = DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.GetCultureInfo("vi-VN"));
                DateTime start = DateTime.ParseExact(startDate, "MM/dd/yyyy", null);
                query = query.Where(x => x.DateCreated >= start);
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                //DateTime end = DateTime.ParseExact(endDate, "dd/MM/yyyy", CultureInfo.GetCultureInfo("vi-VN"));
                DateTime end = DateTime.ParseExact(endDate, "MM/dd/yyyy", null);
                query = query.Where(x => x.DateCreated <= end);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Product.Name.Contains(keyword) || x.Product.Publisher.Contains(keyword));
            }
            var totalRow = query.Count();
            var data = _mapper.ProjectTo<ProductQuantityViewModel>(query.OrderByDescending(x => x.DateCreated)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize))
                .ToList();
            return new PagedResult<ProductQuantityViewModel>()
            {
                CurrentPage = pageIndex,
                PageSize = pageSize,
                Results = data,
                RowCount = totalRow
            };
        }

        public List<ProductQuantityViewModel> GetImport(string publisher, string startDate, string endDate)
        {
            //DateTime start = DateTime.ParseExact(startDate, "MM/dd/yyyy HH:mm:ss", null);
            //DateTime end = DateTime.ParseExact(endDate, "MM/dd/yyyy HH:mm:ss", null);
            DateTime start = DateTime.Parse(startDate);
            DateTime end = DateTime.Parse(endDate);
            if (publisher == null)
            {
                return _mapper.ProjectTo<ProductQuantityViewModel>(_productQuantityRepository.FindAll(x => x.DateCreated >= start && x.DateCreated <= end, c => c.Product)).ToList();
            }
            return _mapper.ProjectTo<ProductQuantityViewModel>(_productQuantityRepository
                .FindAll(x => x.Product.Publisher == publisher
                && x.DateCreated >= start
                && x.DateCreated <= end
                , c => c.Product)).ToList();
        }

    }
}
