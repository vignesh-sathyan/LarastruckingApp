using LarastruckingApp;
using LarastruckingApp.Entities.Permission;
using LarastruckingApp.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
public class IsPermited
{
    #region HasPermission
    /// <summary>
    /// This method will be used to hide add/ delete/ edit button based on role
    /// </summary>
    /// <param name="permission"></param>
    /// <returns></returns>
    public static PermissionsDto HasPermission()
    {
        PermissionsDto permissions = new PermissionsDto();

        try
        {
            MemberProfile mp = new MemberProfile();

            if (mp.Permissions.Count > 0)
            {
                var request = HttpContext.Current.Request;
                string controller = request.RequestContext.RouteData.Values["controller"].ToString();
                var filteredClaims = mp.Permissions.Where(a => a.ControllerName.Equals(controller, StringComparison.InvariantCultureIgnoreCase)).ToList();

                if (filteredClaims.Count() > 0)
                {
                    permissions.IsInsert = filteredClaims.Any(i => i.CanInsert == true);
                    permissions.IsDelete = filteredClaims.Any(i => i.CanDelete == true);
                    permissions.IsUpdate = filteredClaims.Any(i => i.CanUpdate == true);
                    permissions.IsView = filteredClaims.Any(i => i.CanView == true);
                    permissions.IsPricingMethod = filteredClaims.Any(i => i.IsPricingMethod == true);
                }
            }
        }
        catch (Exception)
        {
            return permissions;
        }
        return permissions;
    }
    #endregion

    #region HasPermission
    /// <summary>
    /// This method will find the value of the property passed
    /// </summary>
    /// <param name="data"></param>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    public static bool GetPropValue(List<ActionButtonDto> data, string propertyName)
    {
        foreach (var item in data)
        {
            PropertyInfo[] propertyInfo = item.GetType().GetProperties();

            foreach (var prop in propertyInfo)
            {
                if (prop.Name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase))
                {
                    bool value = Convert.ToBoolean(prop.GetValue(item));

                    if (value)
                    {
                        return value;
                    }
                }
            }
        }

        return false;
    }
    #endregion
}
