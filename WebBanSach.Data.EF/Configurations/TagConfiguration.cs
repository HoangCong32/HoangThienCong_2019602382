using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WebBanSach.Data.EF.Extensions;
using WebBanSach.Data.Entities;

namespace WebBanSach.Data.EF.Configurations
{
	public class TagConfiguration : DbEntityConfiguration<Tag>
	{
		public override void Configure(EntityTypeBuilder<Tag> entity)
		{
			entity.Property(c => c.Id).HasMaxLength(50)
				.IsRequired().IsUnicode(false);
		}
	}
}
