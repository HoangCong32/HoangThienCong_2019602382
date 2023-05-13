using System;
using System.Collections.Generic;
using System.Text;
using WebBanSach.Data.Entities;
using WebBanSach.Data.IRepositories;

namespace WebBanSach.Data.EF.Repositories
{
    public class SystemConfigRepository : EFRepository<SystemConfig, string>, ISystemConfigRepository
    {
        public SystemConfigRepository(AppDbContext dbFactory) : base(dbFactory)
        {
        }
    }
}
