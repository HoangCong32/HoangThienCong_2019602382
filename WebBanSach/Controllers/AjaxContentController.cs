﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBanSach.Controllers
{
	public class AjaxContentController : Controller
	{
		public IActionResult HeaderCart()
		{
			return ViewComponent("HeaderCart");
		}
	}
}
