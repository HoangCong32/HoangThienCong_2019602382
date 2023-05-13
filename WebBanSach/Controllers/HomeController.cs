using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebBanSach.Application.Interfaces;
using WebBanSach.Extensions;
using WebBanSach.Models;

namespace WebBanSach.Controllers
{
	//[Authorize]
	public class HomeController : Controller
	{
		private IProductService _productService;
		private IProductCategoryService _productCategoryService;

		private ICommonService _commonService;
		private readonly IStringLocalizer<HomeController> _localizer;

		public HomeController(IProductService productService,
		ICommonService commonService,
		IProductCategoryService productCategoryService,
		IStringLocalizer<HomeController> localizer)
		{
			_commonService = commonService;
			_productService = productService;
			_productCategoryService = productCategoryService;
			_localizer = localizer;
		}

		//[ResponseCache(CacheProfileName = "Default")]
		public IActionResult Index()
		{
			var title = _localizer["Title"];
			var culture = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;
			ViewData["BodyClass"] = "cms-index-index cms-home-page";
			var homeVm = new HomeViewModel();
			homeVm.HomeCategories = _productCategoryService.GetHomeCategories(5);
			homeVm.HotProducts = _productService.GetHotProduct(5);
			homeVm.TopSellProducts = _productService.GetLastest(5);
			return View(homeVm);
		}

		public IActionResult About()
		{
			ViewData["Message"] = "Your application description page.";

			return View();
		}

		public IActionResult Contact()
		{
			ViewData["Message"] = "Your contact page.";

			return View();
		}

		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		[HttpPost]
		public IActionResult SetLanguage(string culture, string returnUrl)
		{
			Response.Cookies.Append(
				CookieRequestCultureProvider.DefaultCookieName,
				CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
				new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
			);

			return LocalRedirect(returnUrl);
		}
	}
}
