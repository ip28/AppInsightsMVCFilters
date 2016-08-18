using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace AppInsightsMVCFilters
{
    public class TrackExceptionFilterAsync : ExceptionFilterAttribute
    {
        readonly TelemetryHelper _telemetryHelper = new TelemetryHelper();
        private string _methodName;

        public TrackExceptionFilterAsync()
        {
        }
        public TrackExceptionFilterAsync(string methodName)
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
