using System.Net.Http;
using System.Web.Http;

// Mirai APIs can only be called after forms authentication.
// We do not have any token based access to the API at the moment and mobile apps also maintain
// a valid HTTP session.
// NOTE: Although you can access Session, it is READ ONLY, because requiring writeable session
// means that all ajax requests for the same user would get serialized on the server. i.e. having
// readonly access to session improves performance.
public class MiraiAuthorize : AuthorizeAttribute
{
    public override void OnAuthorization(
            System.Web.Http.Controllers.HttpActionContext actionContext)
    {
        var session = System.Web.HttpContext.Current.Session;
        if (session == null || session["UserEmail"] == null) {
            actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
        }
        return;
    }
}