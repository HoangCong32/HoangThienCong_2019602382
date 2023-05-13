using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebBanSach.Application.AutoMapper
{
	public class AutoMapperConfig
	{
		public static MapperConfiguration RegisterMapping()
		{
			return new MapperConfiguration(cfg =>
			{
				cfg.AddProfile(new DomainToViewModelMappingProfile());
				cfg.AddProfile(new ViewModelToDomainMappingProfile());
			});
		}
	}
}
