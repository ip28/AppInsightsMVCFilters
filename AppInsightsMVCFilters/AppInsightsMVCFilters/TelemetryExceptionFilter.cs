using System.Web.Http.Filters;

namespace AppInsightsMVCFilters
{
    public class TrackExceptionFilter : ExceptionFilterAttribute
    {
        readonly TelemetryHelper _telemetryHelper = new TelemetryHelper();
        private string _methodName;

        public TrackExceptionFilter()
        {
        }
        public TrackExceptionFilter(string methodName)
        {
            _methodName = methodName;
        }
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            _methodName = !string.IsNullOrWhiteSpace(_methodName)
                ? _methodName
                : actionExecutedContext?.ActionContext?.ActionDescriptor?.ActionName;
            _telemetryHelper.TrackException(actionExecutedContext.Exception, _methodName);
        }
    }
}
