﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WebBanSach.Application.Interfaces;
using WebBanSach.Application.ViewModels.Product;
using WebBanSach.Data.Entities;
using WebBanSach.Data.Enums;
using WebBanSach.Data.IRepositories;
using WebBanSach.Infrastructure.Interfaces;
using WebBanSach.Utilities.Dtos;

namespace WebBanSach.Application.Implementation
{
	public class BillService : IBillService
	{
		private readonly IBillRepository _orderRepository;
		private readonly IBillDetailRepository _orderDetailRepository;
		private readonly IProductRepository _productRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public BillService(IBillRepository orderRepository,
			IBillDetailRepository orderDetailRepository,
			IProductRepository productRepository,
			IUnitOfWork unitOfWork,
			IMapper mapper)
		{
			_orderRepository = orderRepository;
			_orderDetailRepository = orderDetailRepository;
			_productRepository = productRepository;
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public void Create(BillViewModel billVm)
		{
			var order = _mapper.Map<BillViewModel, Bill>(billVm);
			var orderDetails = _mapper.Map<List<BillDetailViewModel>, List<BillDetail>>(billVm.BillDetails);
			foreach (var detail in orderDetails)
			{
				var product = _productRepository.FindById(detail.ProductId);
				detail.Price = product.Price;;
            }
			order.BillDetails = orderDetails;
			_orderRepository.Add(order);
		}

		public BillDetailViewModel CreateDetail(BillDetailViewModel billDetailVm)
		{
			var billDetail = _mapper.Map<BillDetailViewModel, BillDetail>(billDetailVm);
			_orderDetailRepository.Add(billDetail);
			return billDetailVm;
		}

		public void DeleteDetail(int productId, int billId)
		{
			var detail = _orderDetailRepository.FindSingle(x => x.ProductId == productId && x.BillId == billId);
			_orderDetailRepository.Remove(detail);
		}

		public PagedResult<BillViewModel> GetAllPaging(string startDate, string endDate, string keyword
			, int pageIndex, int pageSize)
		{
			var query = _orderRepository.FindAll();
			if (!string.IsNullOrEmpty(startDate))
			{
				DateTime start = DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.GetCultureInfo("vi-VN"));
				query = query.Where(x => x.DateCreated >= start);
			}
			if (!string.IsNullOrEmpty(endDate))
			{
				DateTime end = DateTime.ParseExact(endDate, "dd/MM/yyyy", CultureInfo.GetCultureInfo("vi-VN"));
				query = query.Where(x => x.DateCreated <= end);
			}
			if (!string.IsNullOrEmpty(keyword))
			{
				query = query.Where(x => x.CustomerName.Contains(keyword) || x.CustomerMobile.Contains(keyword));
			}
			var totalRow = query.Count();
			var data = _mapper.ProjectTo<BillViewModel>(query.OrderByDescending(x => x.DateCreated)
				.Skip((pageIndex - 1) * pageSize)
				.Take(pageSize))
				.ToList();
			return new PagedResult<BillViewModel>()
			{
				CurrentPage = pageIndex,
				PageSize = pageSize,
				Results = data,
				RowCount = totalRow
			};
		}

		public List<BillDetailViewModel> GetBillDetails(int billId)
		{
			return _mapper.ProjectTo<BillDetailViewModel>(_orderDetailRepository
				.FindAll(x => x.BillId == billId, c => c.Bill, c => c.Product)).ToList();
		}

		public BillViewModel GetDetail(int billId)
		{
			var bill = _orderRepository.FindSingle(x => x.Id == billId);
			var billVm = _mapper.Map<Bill, BillViewModel>(bill);
			var billDetailVm = _mapper.ProjectTo<BillDetailViewModel>(_orderDetailRepository.FindAll(x => x.BillId == billId)).ToList();
			billVm.BillDetails = billDetailVm;
			return billVm;
		}

		public void Save()
		{
			_unitOfWork.Commit();
		}

		public void Update(BillViewModel billVm)
		{
			//Mapping to order domain
			var order = _mapper.Map<BillViewModel, Bill>(billVm);

			//Get order Detail
			var newDetails = order.BillDetails;

			//new details added
			var addedDetails = newDetails.Where(x => x.Id == 0).ToList();

			//get updated details
			var updatedDetails = newDetails.Where(x => x.Id != 0).ToList();

			//Existed details
			var existedDetails = _orderDetailRepository.FindAll(x => x.BillId == billVm.Id);

			//Clear db
			order.BillDetails.Clear();

			foreach (var detail in updatedDetails)
			{
				var product = _productRepository.FindById(detail.ProductId);
				detail.Price = product.Price;
				_orderDetailRepository.Update(detail);
			}

			foreach (var detail in addedDetails)
			{
				var product = _productRepository.FindById(detail.ProductId);
				detail.Price = product.Price;
				_orderDetailRepository.Add(detail);
			}

			//_orderDetailRepository.RemoveMultiple(existedDetails.Except(updatedDetails).ToList());

			_orderRepository.Update(order);
		}

		public void UpdateStatus(int billId, BillStatus status)
		{
			var order = _orderRepository.FindById(billId);
			order.BillStatus = status;
			_orderRepository.Update(order);
		}
	}
}
