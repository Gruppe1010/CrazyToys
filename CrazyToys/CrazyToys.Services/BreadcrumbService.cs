using CrazyToys.Entities.DTOs;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace CrazyToys.Services
{
    // https://stackoverflow.com/a/48531023
    public class BreadcrumbService : IViewContextAware
    {
        IList<Breadcrumb> breadcrumbs;

        public void Contextualize(ViewContext viewContext)
        {
            breadcrumbs = new List<Breadcrumb>();

            string area = $"{viewContext.RouteData.Values["area"]}";
            string controller = $"{viewContext.RouteData.Values["controller"]}";
            string action = $"{viewContext.RouteData.Values["action"]}";
            object id = viewContext.RouteData.Values["id"];
            string title = $"{viewContext.ViewBag.Current}";

            breadcrumbs.Add(new Breadcrumb(area, controller, action, title, id));

            if (!string.Equals(action, "index", StringComparison.OrdinalIgnoreCase))
            {
                breadcrumbs.Insert(0, new Breadcrumb(area, controller, "index", title));
            }
        }

        public IList<Breadcrumb> GetBreadcrumbs()
        {
            return breadcrumbs;
        }
    }
}
