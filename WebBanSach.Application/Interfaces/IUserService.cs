using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebBanSach.Application.ViewModels.System;
using WebBanSach.Utilities.Dtos;

namespace WebBanSach.Application.Interfaces
{
    public interface IUserService
    {
        Task<bool> AddAsync(AppUserViewModel userVm);

        Task DeleteAsync(string id);

        Task<List<AppUserViewModel>> GetAllAsync();

        PagedResult<AppUserViewModel> GetAllPagingAsync(string keyword, int page, int pageSize);

        Task<AppUserViewModel> GetById(string id);


        Task UpdateAsync(AppUserViewModel userVm);
    }
}
