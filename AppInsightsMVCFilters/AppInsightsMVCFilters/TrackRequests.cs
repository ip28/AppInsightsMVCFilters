using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace AppInsightsMVCFilters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class TrackRequests : ActionFilterAttribute
    {
        protected readonly TelemetryHelper TelemetryHelper = new TelemetryHelper();
        protected readonly bool LogPayload = GlobalConfiguration.Instance.LogPayload;
        protected string MethodName;
        protected string ExceptionMethodName;

        public TrackRequests()
        {
        }
        public TrackRequests(string methodName)
        {
            MethodName = methodName;
            ExceptionMethodName = $"OnActionExecuted of method- {MethodName}";
        }
      

        public override void OnActionExecuting(HttpActionContext actionContext)

        {
            try
            {
                TelemetryHelper.Start(MethodName);
                if (LogPayload)
                {
                    var url = actionContext.RequestContext.Url.Request.RequestUri.AbsoluteUri;
                    TelemetryHelper.TrackRequestUri("Request",url);
                }
            }
            catch (Exception ex)
            {
                LogRequestException(actionContext, ex);
            }
        }

        protected void LogRequestException(HttpActionContext actionContext, Exception ex)
        {
            ExceptionMethodName = !string.IsNullOrWhiteSpace(ExceptionMethodName)
                ? ExceptionMethodName
                : actionContext?.ActionDescriptor?.ActionName;
            TelemetryHelper.TrackException(ex, ExceptionMethodName);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            try
            {
                MethodName = !string.IsNullOrWhiteSpace(MethodName)
                        ? MethodName
                        : actionExecutedContext?.ActionContext?.ActionDescriptor?.ActionName;
                var httpStatusCode = (Convert.ToInt32(actionExecutedContext.Response.StatusCode)).ToString();
                var isCallSuccess = actionExecutedContext.Response.IsSuccessStatusCode;
                TelemetryHelper.TrackRequest(httpStatusCode, MethodName, isCallSuccess);
                if (LogPayload)
                {
                    TelemetryHelper.LogPayload(actionExecutedContext.Response.Content,"Response");
                }
            }
            catch (Exception ex)
            {
                LogResponseException(actionExecutedContext, ex);
            }
        }

        protected void LogResponseException(HttpActionExecutedContext actionExecutedContext, Exception ex)
        {
            ExceptionMethodName = !string.IsNullOrWhiteSpace(ExceptionMethodName)
                ? ExceptionMethodName
                : actionExecutedContext?.ActionContext?.ActionDescriptor?.ActionName;
            TelemetryHelper.TrackException(ex, ExceptionMethodName);
        }
    }


}
