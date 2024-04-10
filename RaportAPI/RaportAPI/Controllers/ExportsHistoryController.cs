using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RaportAPI.Core.Application.Raports.Queries.GetRaports;
using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace RaportAPI.Controllers
{
    [Route("api/exports-history")]
    [ApiController]
    public class ExportsHistoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExportsHistoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(Name = "GetExportsHistory")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ExportHistoryVm))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ExportHistoryVm>> GetExportsHistory([FromQuery][Required][Range(1, int.MaxValue)] int page,
                                            [FromQuery][Required][Range(1, int.MaxValue)] int range,
                                            string? locationName, string? startDate, string? endDate, CancellationToken cancellationToken)
        {
            DateOnly? startDateOnly = null;
            DateOnly? endDateOnly = null;

            if (startDate != null) 
            { 
                try
                {
                    startDateOnly = DateOnly.Parse(startDate);
                }
                catch (Exception) 
                {
                    return BadRequest("Cannot convert start date to type dateOnly!");
                }   
            }
            if (endDate != null)
            {
                try
                {
                    endDateOnly = DateOnly.Parse(endDate);
                }
                catch (Exception)
                {
                    return BadRequest("Cannot convert end date to type dateOnly!");
                }
            }
            if (startDateOnly != null && startDateOnly > endDateOnly) return BadRequest("Start date cannot be greater than end date!");
            if (endDateOnly != null && startDateOnly > endDateOnly) return BadRequest("End date cannot be smaller then start date!");

            var result = await _mediator.Send(new GetExportHistoryQuery() 
            { 
                LocationName = locationName, 
                StartDate = startDateOnly, 
                EndDate = endDateOnly,
                PageNumber = page,
                Range = range,
            }, cancellationToken);

            return result;
        }
    }
}
