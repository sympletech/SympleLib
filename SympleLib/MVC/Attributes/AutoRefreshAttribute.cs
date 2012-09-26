using System;
using System.Security.Permissions;
using System.Web;
using System.Web.Mvc;

namespace SympleLib.MVC.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class AutoRefreshAttribute : ActionFilterAttribute
    {
        public const int DefaultDurationInSeconds = 300; // 5 Minutes

        public AutoRefreshAttribute()
        {
            DurationInSeconds = DefaultDurationInSeconds;
        }

        public int DurationInSeconds { get; set; }

        public string RouteName { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            string url = BuildUrl(filterContext);
            string headerValue = string.Concat(DurationInSeconds, ";Url=", url);

            filterContext.HttpContext.Response.AppendHeader("Refresh", headerValue);

            base.OnResultExecuted(filterContext);
        }

        private string BuildUrl(ControllerContext filterContext)
        {
            UrlHelper urlHelper = new UrlHelper(filterContext.RequestContext);
            string url;

            if (!string.IsNullOrEmpty(RouteName))
            {
                url = urlHelper.RouteUrl(RouteName);
            }
            else if (!string.IsNullOrEmpty(ControllerName) && !string.IsNullOrEmpty(ActionName))
            {
                url = urlHelper.Action(ActionName, ControllerName);
            }
            else if (!string.IsNullOrEmpty(ActionName))
            {
                url = urlHelper.Action(ActionName);
            }
            else
            {
                url = filterContext.HttpContext.Request.RawUrl;
            }

            url += "?" + filterContext.HttpContext.Request.QueryString;
            url = url.TrimEnd('?');

            return url;
        }
    }
}