using MediatR;
using System.Diagnostics;

namespace RaportAPI.Core.Application.Common.Behaviours
{
    public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger _logger;
        private readonly System.Diagnostics.Stopwatch _timer;
        public PerformanceBehaviour(ILogger<TRequest> logger)
        {
            _timer = new Stopwatch();
            _logger = logger;
        }

        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _timer.Start();

            var response = next();

            _timer.Stop();

            var elapsed = _timer.ElapsedMilliseconds;

            if (elapsed > 1000)
            {
                var requestName = typeof(TRequest).Name;

                _logger.LogInformation("Long Running Request: {Name} ({elapsed} milliseconds) {@Request}",
                    requestName, elapsed, request);
            }

            return response;
        }
    }
}
