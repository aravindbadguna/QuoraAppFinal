using QuoraApp.ServiceLayer;
using System.Web.Http;

namespace QuoraApp.UI.ApiControllers
{
    public class AccountController : ApiController
    {
        IUsersService us;
        public AccountController(IUsersService us)
        {
            this.us = us;
        }

        public string Get(string Email)
        {
            if(this.us.GetUsersByEmail(Email) != null)
            {
                return "Found";
            }
            else
            {
                return "Not Found";
            }
        }
    }
}
