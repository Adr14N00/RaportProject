using RaportAPI.Core.Application.Raports.Queries.GetRaports;

namespace RaportAPI.Core.Application.Common.Interfaces
{
    public interface IExportsHistoryService
    {
        Task<ExportHistoryVm> GetExportsHistoryAsync(GetExportHistoryQuery query, CancellationToken cancellationToken);
    }
}
