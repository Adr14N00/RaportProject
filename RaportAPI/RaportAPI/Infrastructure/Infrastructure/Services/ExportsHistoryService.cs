using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RaportAPI.Core.Application.Common.Interfaces;
using RaportAPI.Core.Application.Raports.Queries.GetRaports;
using RaportAPI.Core.Domain.Entities;

namespace RaportAPI.Infrastructure.Infrastructure.Services
{
    public class ExportsHistoryService : IExportsHistoryService
    {
        private readonly IRaportsDbContext _context;
        private readonly IMapper _mapper;

        public ExportsHistoryService(IRaportsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async virtual Task<ExportHistoryVm> GetExportsHistoryAsync(GetExportHistoryQuery query, CancellationToken cancellationToken)
        {
            var dbQuery = _context.ExportHistory.AsQueryable();

            if (query.LocationName != null) dbQuery = dbQuery.Where(e => e.LocationName.ToLower().Equals(query.LocationName.ToLower()));
            if (query.StartDate != null)
            {
                DateOnly startDate = query.StartDate.GetValueOrDefault();
                DateTime startDateTime = startDate.ToDateTime(TimeOnly.MinValue);
                startDateTime = startDateTime.Date; 
                dbQuery = dbQuery.Where(e => e.Exportdatetime >= startDateTime);
            }
            if (query.EndDate != null)
            {
                DateOnly endDate = query.EndDate.GetValueOrDefault();
                DateTime endDateTime = endDate.ToDateTime(TimeOnly.MaxValue);
                endDateTime = endDateTime.Date.AddDays(1).AddTicks(-1);
                dbQuery = dbQuery.Where(e => e.Exportdatetime <= endDateTime);
            }



            var dbQueryRecordCount = dbQuery.Count();
            dbQuery = dbQuery.OrderBy(e => e.Exportdatetime);

            if (query.PageNumber == 1) dbQuery = dbQuery.Take(query.Range);
            else dbQuery = dbQuery.Skip((query.PageNumber - 1) * query.Range).Take(query.Range);

            var exports = await dbQuery.AsNoTracking().ToListAsync(cancellationToken);

            return new ExportHistoryVm()
            {
                NumberOfItems = dbQueryRecordCount,
                Exports = _mapper.Map<ICollection<ExportHistoryDto>>(exports),
            };
        }


    }
}
