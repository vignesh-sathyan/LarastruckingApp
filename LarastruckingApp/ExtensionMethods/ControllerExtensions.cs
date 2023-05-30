using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LarastruckingApp.Entities.Driver;

public static class ControllerExtensions
{
    #region Render View to String
    /// <summary>
    /// Extension method to render view into string
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <param name="controller"></param>
    /// <param name="viewNamePath"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    public static string RenderViewToStringAsync<TModel>(this Controller controller, string viewNamePath, TModel model)
    {
        controller.ViewData.Model = model;
        using (var sw = new StringWriter())
        {
            var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewNamePath);
            var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
            viewResult.View.Render(viewContext, sw);
            viewResult.ViewEngine.ReleaseView(controller.ControllerContext, viewResult.View);
            return sw.GetStringBuilder().ToString();
        }
    }
    #endregion

    #region DropDown Extension Binding
    /// <summary>
    /// This is an extension method to bind dropdown with enum
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static SelectList ToSelectList<TEnum>(this TEnum obj)
          where TEnum : struct, IComparable, IFormattable, IConvertible
    {
        return new SelectList(Enum.GetValues(typeof(TEnum))
        .OfType<Enum>()
        .Select(x => new SelectListItem
        {
            Text = Enum.GetName(typeof(TEnum), x),
            Value = Enum.GetName(typeof(TEnum), x)
            .ToString()
        }), "Value", "Text");
    }

    #endregion
}
