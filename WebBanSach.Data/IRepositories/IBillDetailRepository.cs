using System;
using System.Collections.Generic;
using System.Text;
using WebBanSach.Data.Entities;
using WebBanSach.Infrastructure.Interfaces;

namespace WebBanSach.Data.IRepositories
{
    public interface IBillDetailRepository : IRepository<BillDetail, int>
    {
    }
}
