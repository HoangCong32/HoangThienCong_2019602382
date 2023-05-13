using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebBanSach.Application.Dapper.ViewModels;

namespace WebBanSach.Application.Dapper.Interfaces
{
    public interface IReportService
    {
        Task<IEnumerable<RevenueReportViewModel>> GetReportAsync(string fromDate, string toDate);
    }
}
