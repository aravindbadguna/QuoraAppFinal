using System.Web.Mvc;

namespace QuoraApp.UI.CustomFilters
{
    public class UseAuthorizationFilterAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if(filterContext.RequestContext.HttpContext.Session["CurrentUserName"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new
                {
                    controller="Account",action="Login"
                }));
            }
        }
    }
}