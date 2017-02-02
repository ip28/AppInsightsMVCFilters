using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace AppInsightsMVCFilters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class TrackRequests : ActionFilterAttribute
    {
        readonly TelemetryHelper _telemetryHelper = new TelemetryHelper();
        private readonly bool _logPayload = GlobalConfiguration.Instance.LogPayload;
        private string _methodName;
        private string _exceptionMethodName;

        public TrackRequests()
        {
        }
        public TrackRequests(string methodName)
        {
            _methodName = methodName;
            _exceptionMethodName = $"OnActionExecuted of method- {_methodName}";
        }
      

        public override void OnActionExecuting(HttpActionContext actionContext)

        {
            try
            {
                _telemetryHelper.Start(_methodName);
                if (_logPayload)
                {
                    var url = actionContext.RequestContext.Url.Request.RequestUri.AbsoluteUri;
                    //_telemetryHelper.TrackRequestUri("Request",actionContext.RequestContext.Url);
                    _telemetryHelper.LogPayload(actionContext.Request.Content, "Request");
                }
            }
            catch (Exception ex)
            {
                _exceptionMethodName = !string.IsNullOrWhiteSpace(_exceptionMethodName)
                    ? _exceptionMethodName
                    : actionContext?.ActionDescriptor?.ActionName;
                _telemetryHelper.TrackException(ex, _exceptionMethodName);
            }
        }
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            try
            {
                _methodName = !string.IsNullOrWhiteSpace(_methodName)
                        ? _methodName
                        : actionExecutedContext?.ActionContext?.ActionDescriptor?.ActionName;
                var httpStatusCode = (Convert.ToInt32(actionExecutedContext.Response.StatusCode)).ToString();
                var isCallSuccess = actionExecutedContext.Response.IsSuccessStatusCode;
                _telemetryHelper.TrackRequest(httpStatusCode, _methodName, isCallSuccess);
                if (_logPayload)
                {
                    _telemetryHelper.LogPayload(actionExecutedContext.Response.Content,"Response");
                }
            }
            catch (Exception ex)
            {
                _exceptionMethodName = !string.IsNullOrWhiteSpace(_exceptionMethodName)
                    ? _exceptionMethodName
                    : actionExecutedContext?.ActionContext?.ActionDescriptor?.ActionName; 
                _telemetryHelper.TrackException(ex,_exceptionMethodName);
            }
        }

    }


}
