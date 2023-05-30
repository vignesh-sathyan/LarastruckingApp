using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace LarastruckingApp.Infrastructure
{
    public class IdentityHelper
    {
        #region Private Members
        /// <summary>
        /// Defining AuthenticationManager
        /// </summary>
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().Authentication;
            }
        }
        #endregion

        #region Constructor
        public IdentityHelper()
        {
        }
        #endregion
       
        #region IdentitySignin
        /// <summary>
        /// Setting up claims
        /// </summary>
        /// <param name="claimProperty"></param>
        /// <param name="isPersistent"></param>
        public void IdentitySignin(ClaimsTypeProperty claimProperty, bool isPersistent = false)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, claimProperty.Identity_UserName));
            claims.Add(new Claim("UserId", claimProperty.Identity_UserId.ToString()));
            claims.Add(new Claim("FullName", claimProperty.Identity_FullName.ToString()));

            claims.Add(new Claim("Permission", JsonConvert.SerializeObject(claimProperty.Identity_Permissions)));

            foreach (var r in claimProperty.Identity_Role)
            {
                claims.Add(new Claim(ClaimTypes.Role, r));
            }

            var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = isPersistent,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
            }, identity);
        }
        #endregion
       
        #region IdentitySignout
        /// <summary>
        /// IdentitySignout
        /// </summary>
        public void IdentitySignout()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie,
                                          DefaultAuthenticationTypes.ExternalCookie);
        }
        #endregion

    }
}