using MediatR;
using RaportAPI.Core.Application.Common.Interfaces;

namespace RaportAPI.Core.Application.Raports.Queries.GetRaports
{
    public class GetExportHistoryQueryHandler : IRequestHandler<GetExportHistoryQuery, ExportHistoryVm>
    {
        private readonly IExportsHistoryService _exportsHistoryService;

        public GetExportHistoryQueryHandler(IExportsHistoryService exportsHistoryService)
        {
            _exportsHistoryService = exportsHistoryService;
        }

        public async Task<ExportHistoryVm> Handle(GetExportHistoryQuery request, CancellationToken cancellationToken)
        {
            return await _exportsHistoryService.GetExportsHistoryAsync(request, cancellationToken);
        }
    }
}
