using System;
using System.Collections.Generic;
using System.Text;
using WebBanSach.Data.Enums;

namespace WebBanSach.Data.Interfaces
{
	public interface ISwitchable
	{
		Status Status { get; set; }
	}
}
