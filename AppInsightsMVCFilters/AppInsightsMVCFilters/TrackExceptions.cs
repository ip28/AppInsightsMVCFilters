using System;
using System.Web.Http.Filters;

namespace AppInsightsMVCFilters
{
    public class TrackExceptions : ExceptionFilterAttribute
    {
        readonly TelemetryHelper _telemetryHelper = new TelemetryHelper();
        private string _methodName;
        private string _exceptionMethodName;

        public TrackExceptions()
        {
        }
        public TrackExceptions(string methodName)
        {
            _methodName = methodName;
            _exceptionMethodName = $"OnActionExecuted of method- {_methodName}";
        }
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            try
            {
                _methodName = !string.IsNullOrWhiteSpace(_methodName)
                       ? _methodName
                       : actionExecutedContext?.ActionContext?.ActionDescriptor?.ActionName;
                _telemetryHelper.TrackException(actionExecutedContext.Exception, _methodName);
            }
            catch (Exception ex)
            {
                _exceptionMethodName = !string.IsNullOrWhiteSpace(_exceptionMethodName)
                     ? _exceptionMethodName
                     : actionExecutedContext?.ActionContext?.ActionDescriptor?.ActionName;
                _telemetryHelper.TrackException(ex, _exceptionMethodName);
            }
        }
    }
}
