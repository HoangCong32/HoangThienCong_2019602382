using System;
using System.Collections.Generic;
using System.Text;
using WebBanSach.Data.Entities;
using WebBanSach.Data.IRepositories;

namespace WebBanSach.Data.EF.Repositories
{
    public class BillRepository : EFRepository<Bill, int>, IBillRepository
    {
        public BillRepository(AppDbContext context) : base(context)
        {
        }
    }
}
