using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanSach.Application.Interfaces;
using WebBanSach.Application.ViewModels.System;
using WebBanSach.Data.Entities;
using WebBanSach.Data.Enums;
using WebBanSach.Data.Interfaces;
using WebBanSach.Data.IRepositories;
using WebBanSach.Infrastructure.Interfaces;

namespace WebBanSach.Application.Implementation
{
	public class FunctionService : IFunctionService
	{
		private IFunctionRepository _functionRepository;
		private IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public FunctionService(IMapper mapper,
			IFunctionRepository functionRepository,
			IUnitOfWork unitOfWork)
		{
			_functionRepository = functionRepository;
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public void Add(FunctionViewModel functionVm)
		{
			var function = _mapper.Map<Function>(functionVm);
			_functionRepository.Add(function);
		}

		public bool CheckExistedId(string id)
		{
			return _functionRepository.FindById(id) != null;
		}

		public void Delete(string id)
		{
			_functionRepository.Remove(id);
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}

		public Task<List<FunctionViewModel>> GetAll(string filter)
		{
			var query = _functionRepository.FindAll(x => x.Status == Status.Active);
			if (!string.IsNullOrEmpty(filter))
				query = query.Where(x => x.Name.Contains(filter));
			return _mapper.ProjectTo<FunctionViewModel>(query.OrderBy(x => x.ParentId)).ToListAsync();
		}

		public IEnumerable<FunctionViewModel> GetAllWithParentId(string parentId)
		{
			return _mapper.ProjectTo<FunctionViewModel>(
				_functionRepository.FindAll(x => x.ParentId == parentId));
		}

		public FunctionViewModel GetById(string id)
		{
			var function = _functionRepository.FindSingle(x => x.Id == id);
			return _mapper.Map<Function, FunctionViewModel>(function);
		}

		public void ReOrder(string sourceId, string targetId)
		{

			var source = _functionRepository.FindById(sourceId);
			var target = _functionRepository.FindById(targetId);
			int tempOrder = source.SortOrder;

			source.SortOrder = target.SortOrder;
			target.SortOrder = tempOrder;

			_functionRepository.Update(source);
			_functionRepository.Update(target);

		}

		public void Save()
		{
			_unitOfWork.Commit();
		}

		public void Update(FunctionViewModel functionVm)
		{

			var functionDb = _functionRepository.FindById(functionVm.Id);
			var function = _mapper.Map<Function>(functionVm);
		}

		public void UpdateParentId(string sourceId, string targetId, Dictionary<string, int> items)
		{
			//Update parent id for source
			var category = _functionRepository.FindById(sourceId);
			category.ParentId = targetId;
			_functionRepository.Update(category);

			//Get all sibling
			var sibling = _functionRepository.FindAll(x => items.ContainsKey(x.Id));
			foreach (var child in sibling)
			{
				child.SortOrder = items[child.Id];
				_functionRepository.Update(child);
			}
		}
	}
}
