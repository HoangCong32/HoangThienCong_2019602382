using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebBanSach.Application.Interfaces;
using WebBanSach.Application.ViewModels.Blog;
using WebBanSach.Data.Entities;
using WebBanSach.Data.IRepositories;
using WebBanSach.Infrastructure.Interfaces;
using WebBanSach.Utilities.Dtos;

namespace WebBanSach.Application.Implementation
{
	public class PageService : IPageService
	{
		private IPageRepository _pageRepository;
		private IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public PageService(IPageRepository pageRepository,
			IUnitOfWork unitOfWork,
			IMapper mapper)
		{
			this._pageRepository = pageRepository;
			this._unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public void Add(PageViewModel pageVm)
		{
			var page = _mapper.Map<PageViewModel, Page>(pageVm);
			_pageRepository.Add(page);
		}

		public void Delete(int id)
		{
			_pageRepository.Remove(id);
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}

		public List<PageViewModel> GetAll()
		{
			return _mapper.ProjectTo<PageViewModel>(_pageRepository.FindAll()).ToList();
		}

		public PagedResult<PageViewModel> GetAllPaging(string keyword, int page, int pageSize)
		{
			var query = _pageRepository.FindAll();
			if (!string.IsNullOrEmpty(keyword))
				query = query.Where(x => x.Name.Contains(keyword));

			int totalRow = query.Count();
			var data = query.OrderByDescending(x => x.Alias)
				.Skip((page - 1) * pageSize)
				.Take(pageSize);

			var paginationSet = new PagedResult<PageViewModel>()
			{
				Results = _mapper.ProjectTo<PageViewModel>(data).ToList(),
				CurrentPage = page,
				RowCount = totalRow,
				PageSize = pageSize
			};

			return paginationSet;
		}

		public PageViewModel GetByAlias(string alias)
		{
			return _mapper.Map<Page, PageViewModel>(_pageRepository.FindSingle(x => x.Alias == alias));
		}

		public PageViewModel GetById(int id)
		{
			return _mapper.Map<Page, PageViewModel>(_pageRepository.FindById(id));
		}

		public void SaveChanges()
		{
			_unitOfWork.Commit();
		}

		public void Update(PageViewModel pageVm)
		{
			var page = _mapper.Map<PageViewModel, Page>(pageVm);
			_pageRepository.Update(page);
		}
	}
}
