using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WebBanSach.Infrastructure.SharesKernel;

namespace WebBanSach.Data.Entities
{
	public class Tag : DomainEntity<string>
	{
		[MaxLength(50)]
		[Required]
		public string Name { get; set; }

		[MaxLength(50)]
		[Required]
		public string Type { get; set; }
	}
}
