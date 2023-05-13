using System;
using System.Collections.Generic;
using System.Text;
using WebBanSach.Application.ViewModels.Common;
using WebBanSach.Utilities.Dtos;

namespace WebBanSach.Application.Interfaces
{
	public interface IFeedbackService
	{
        void Add(FeedbackViewModel feedbackVm);

        void Update(FeedbackViewModel feedbackVm);

        void Delete(int id);

        List<FeedbackViewModel> GetAll();

        PagedResult<FeedbackViewModel> GetAllPaging(string keyword, int page, int pageSize);

        FeedbackViewModel GetById(int id);

        void SaveChanges();
    }
}
