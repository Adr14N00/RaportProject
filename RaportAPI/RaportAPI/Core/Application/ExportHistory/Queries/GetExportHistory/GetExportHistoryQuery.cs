using MediatR;
using RaportAPI.Core.Common.Exceptions;

namespace RaportAPI.Core.Application.Raports.Queries.GetRaports
{
    public class GetExportHistoryQuery : IRequest<ExportHistoryVm>
    {
        private int range = 10;
        public int Range
        {
            get { return range; }
            set
            {
                if (value > 50) range = 50;
                else range = value;
            }
        }
        public int PageNumber { get; set; }
        public string? LocationName { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
    }
}
