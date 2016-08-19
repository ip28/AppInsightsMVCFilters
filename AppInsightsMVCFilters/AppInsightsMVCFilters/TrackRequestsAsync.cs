using System;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace AppInsightsMVCFilters
{
    /// <summary>
    /// Async Track requests (beta feature)
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class TrackRequestsAsync : ActionFilterAttribute
    {
        readonly TelemetryHelper _telemetryHelper = new TelemetryHelper();
        private string _methodName;

        public TrackRequestsAsync()
        {

        }
        public TrackRequestsAsync(string methodName)
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
            var httpStatusCode = (Convert.ToInt32(actionExecutedContext.Response.StatusCode)).ToString();
            var isCallSuccess = actionExecutedContext.Response.IsSuccessStatusCode;
            Task.Factory.StartNew(()=>_telemetryHelper.TrackRequest(httpStatusCode, _methodName, isCallSuccess));
        }

    }


}
