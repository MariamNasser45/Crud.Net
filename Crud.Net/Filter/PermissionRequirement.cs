using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;
namespace Crud.Net.Filter
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        // name of policy which recieved from user 

        public string permission { get; private set; }

        // making its constructor to set recieved permission

        public PermissionRequirement(string Permision)
        {
            permission = Permision;
        }
    }
}
