using System;
using System.Collections.Generic;
using System.Text;

namespace WebBanSach.Data.Interfaces
{
	public interface IHasSoftDelete
	{
		bool IDeleted { get; set; }
	}
}
