namespace RaportAPI.Core.Application.Raports.Queries.GetRaports
{
    public class ExportHistoryVm
    {
        public ICollection<ExportHistoryDto>? Exports { get; set; }
        public int? NumberOfItems { get; set; }
    }
}
