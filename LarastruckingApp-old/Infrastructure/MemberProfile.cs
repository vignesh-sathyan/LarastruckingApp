using LarastruckingApp.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace LarastruckingApp.Infrastructure
{
    public  class MemberProfile
    {
        ClaimsIdentity Cidentity = (ClaimsIdentity)HttpContext.Current.User.Identity;
        public int UserId
        {
            get
            {
                Claim Mp_UserId = Cidentity.FindFirst("UserId");
                return Mp_UserId == null ? 0 : Convert.ToInt32(Mp_UserId.Value);
            }
        }
        public string FullName
        {
            get
            {
                Claim Mp_FullName = Cidentity.FindFirst("FullName");
                return Mp_FullName == null ? null : Mp_FullName.Value;
            }
        }
        public  string UserName
        {
            get
            {
                Claim Mp_UserName = Cidentity.FindFirst(ClaimTypes.Name);
                return Mp_UserName == null ? null : Mp_UserName.Value;
            }
        }
        public string UserRole
        {
            get
            {
                Claim Mp_Role = Cidentity.FindFirst(ClaimTypes.Role);
                return Mp_Role == null ? null : Mp_Role.Value;
            }
        }
        public List<ActionButtonDto> Permissions
        {
            get
            {
                Claim Mp_Permissions = Cidentity.FindFirst("Permission");
                return Mp_Permissions == null ? null : JsonConvert.DeserializeObject<List<ActionButtonDto>>(Mp_Permissions.Value);
            }
        }

    }
}