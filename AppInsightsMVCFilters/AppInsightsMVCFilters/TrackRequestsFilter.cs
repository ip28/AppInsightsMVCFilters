using System;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace AppInsightsMVCFilters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class TrackRequestsFilter : ActionFilterAttribute
    {
        readonly TelemetryHelper _telemetryHelper = new TelemetryHelper();
        private string _methodName;

        public TrackRequestsFilter()
        {

        }
        public TrackRequestsFilter(string methodName)
        {
            _methodName = methodName;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            _telemetryHelper.Start(_methodName);
        }
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            _methodName = !string.IsNullOrWhiteSpace(_methodName)
                  ? _methodName
                  : actionExecutedContext?.ActionContext?.ActionDescriptor?.ActionName;
            _telemetryHelper.TrackRequest();
        }

    }


}
