using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Jwt.AuthManagement
{
    /// <summary>
    /// 权限承载实体
    /// </summary>
    public class PolicyRequirement: IAuthorizationRequirement
    {
        public List<UserPermission> UserPermissions { get; private set; }
        public string DeniedAction { get; set; }
        public PolicyRequirement()
        {
            DeniedAction = new PathString("api/nopermission");
            UserPermissions = new List<UserPermission> { new UserPermission { Url="api/values", UserName="admin" } };
        }
    }

    /// <summary>
    /// 用户权限承载实体
    /// </summary>
    public class UserPermission
    {
        /// <summary>
        /// username
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// request URL
        /// </summary>
        public string Url { get; set; }
    }

}
