using AutoMapper;
using RaportAPI.Core.Application.Common.Mapping;
using RaportAPI.Core.Domain.Entities;

namespace RaportAPI.Core.Application.Raports.Queries.GetRaports
{
    public class ExportHistoryDto : IMapFrom<ExportHistory>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public string? UserName { get; set; }
        public string? LocationName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ExportHistory, ExportHistoryDto>(MemberList.Destination)
                .ForMember(c => c.Id, map => map.MapFrom(src => src.Id))
                .ForMember(c => c.Name, map => map.MapFrom(src => src.Exportname))
                .ForMember(c => c.Date, map => map.MapFrom(src => DateOnly.FromDateTime(src.Exportdatetime)))
                .ForMember(c => c.Time, map => map.MapFrom(src => TimeOnly.FromDateTime(src.Exportdatetime)))
                .ForMember(c => c.UserName, map => map.MapFrom(src => src.Username))
                .ForMember(c => c.LocationName, map => map.MapFrom(src => src.LocationName));
        }

    }
}
