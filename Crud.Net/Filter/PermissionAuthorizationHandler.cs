using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;
namespace Crud.Net.Filter
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {

        public PermissionAuthorizationHandler()     // create its constructor

        {

        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirment)
        {
            //cheack if useres loged in or not

            if (context.User == null)    // if true : there are no any user loged in then this user cannot work with this action
                return;

            // if false : meaning that this user already loged in then
            // cheack all claims of here to determine he has permission to access required actions or not

            // CanAccess return true : if user has claims make this user cabability to access requiremen action

            var CanAccess = context.User.Claims.Any(c => c.Type == "Permisions" && c.Value == requirment.permission && c.Issuer == "LOCAL AUTHORITY"); //type "defined in db" , value"recieved from user"

            if (CanAccess)
            {
                //if true then this user can access requirement action

                context.Succeed(requirment);
                return;
            }



        }





    }
}
