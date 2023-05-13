using System;
using System.Collections.Generic;
using System.Text;
using WebBanSach.Application.ViewModels.Common;

namespace WebBanSach.Application.Interfaces
{
    public interface ICommonService
    {
        FooterViewModel GetFooter();
        SystemConfigViewModel GetSystemConfig(string code);
    }
}
