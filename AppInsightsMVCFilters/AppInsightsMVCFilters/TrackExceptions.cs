using System.Web.Http.Filters;

namespace AppInsightsMVCFilters
{
    public class TrackExceptions : ExceptionFilterAttribute
    {
        readonly TelemetryHelper _telemetryHelper = new TelemetryHelper();
        private string _methodName;

        public TrackExceptions()
        {
        }
        public TrackExceptions(string methodName)
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
