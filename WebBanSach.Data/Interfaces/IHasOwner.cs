using System;
using System.Collections.Generic;
using System.Text;

namespace WebBanSach.Data.Interfaces
{
	public interface IHasOwner<T>
	{
		T OwnerID { get; set; }

	}
}
