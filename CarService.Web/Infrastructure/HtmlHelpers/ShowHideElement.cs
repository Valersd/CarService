using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarService.Web.Infrastructure.HtmlHelpers
{
    public static class ShowHideElement
    {
        public static MvcHtmlString ShowHide(string id, bool showOnInitialization, string additionalCssClass = null, string textForShow = "[ Show ]", string textForHide = "[ Hide ]")
        {
            string cssClass = "show-hide-link";
            if (!string.IsNullOrEmpty(additionalCssClass))
            {
                cssClass += " " + additionalCssClass;
            }

            TagBuilder container = new TagBuilder("a");
            container.AddCssClass(cssClass);
            container.MergeAttribute("id", id);
            container.MergeAttribute("href", "javascript:void(0);");

            TagBuilder forShow = new TagBuilder("em");
            forShow.InnerHtml = textForShow;
            TagBuilder forHide = new TagBuilder("em");
            forHide.InnerHtml = textForHide;

            if (showOnInitialization)
            {
                forShow.MergeAttribute("style", "display:none");
            }
            else
            {
                forHide.MergeAttribute("style", "display:none");
            }

            container.InnerHtml += forShow;
            container.InnerHtml += forHide;

            return MvcHtmlString.Create(container.ToString());
        }
    }
}