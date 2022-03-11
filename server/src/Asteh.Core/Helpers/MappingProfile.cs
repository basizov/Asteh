using Asteh.Core.Models;
using Asteh.Domain.Entities;
using AutoMapper;

namespace Asteh.Core.Helpers
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<UserEntity, UserModel>()
				.ForMember(d => d.LastVisitDate, o => o.MapFrom(s => s.LastVisitDate.ToString("dd.MM.yyyy")))
				.ForMember(
					d => d.TypeName,
					o => o.MapFrom(s => s.Type != null ? s.Type.Name : "-"));
		}
	}
}
