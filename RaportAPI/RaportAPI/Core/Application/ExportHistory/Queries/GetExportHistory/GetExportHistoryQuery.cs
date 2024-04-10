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
        //{
        //    get { return StartDate; }
        //    set
        //    {
        //        if (StartDate != null && StartDate > EndDate)
        //        {
        //            throw new BadValidationException("Start date cannot be greater than end date!");
        //        }
        //        else StartDate = value;
        //    }
        //}
        public DateOnly? EndDate { get; set; }
        //{
        //    get { return EndDate; }
        //    set
        //    {
        //        if (EndDate != null && EndDate < StartDate)
        //        {
        //            throw new BadValidationException("End date cannot be smaller then start date!");
        //        }
        //        else EndDate = value;
        //    }
        //}
    }
}
