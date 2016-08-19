using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace AppInsightsMVCFilters
{
    /// <summary>
    /// Async track exception (beta feature)
    /// </summary>
    public class TrackExceptionsAsync : ExceptionFilterAttribute
    {
        readonly TelemetryHelper _telemetryHelper = new TelemetryHelper();
        private string _methodName;

        public TrackExceptionsAsync()
        {
        }
        public TrackExceptionsAsync(string methodName)
        {
            _methodName = methodName;
        }
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            _methodName = !string.IsNullOrWhiteSpace(_methodName) ? _methodName : actionExecutedContext?.ActionContext?.ActionDescriptor?.ActionName;
            var exception = actionExecutedContext?.Exception;
            Task.Factory.StartNew(() =>
            {
                _telemetryHelper.TrackException(exception, _methodName);
            });

        }
    }
}
