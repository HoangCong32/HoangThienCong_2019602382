﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanSach.Application.ViewModels.Common;

namespace WebBanSach.Models
{
	public class ContactPageViewModel
	{
		public ContactViewModel Contact { set; get; }

		public FeedbackViewModel Feedback { set; get; }
	}
}
