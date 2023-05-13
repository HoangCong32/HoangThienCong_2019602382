using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebBanSach.Application.Interfaces;
using WebBanSach.Application.ViewModels.System;
using WebBanSach.Extensions;
using WebBanSach.Utilities.Constant;
using WebBanSach.Helpers;

namespace WebBanSach.Areas.Admin.Components
{
	public class SideBarViewComponent : ViewComponent
	{
        private IFunctionService _functionService;

        public SideBarViewComponent(IFunctionService functionService)
        {
            _functionService = functionService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var roles = ((ClaimsPrincipal)User).GetSpecificClaim("Roles");
            List<FunctionViewModel> functions;
            if (roles.Split(";").Contains(CommonConstants.AppRole.AdminRole))
            {
                functions = await _functionService.GetAll(string.Empty);
            }
            else
            {
                //TODO: Get by permission
                //functions = new List<FunctionViewModel>();
                functions = await _functionService.GetAll(string.Empty);
            }
            return View(functions);
        }
    }
}
