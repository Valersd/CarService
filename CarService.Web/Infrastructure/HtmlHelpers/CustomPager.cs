using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarService.Web.Infrastructure.HtmlHelpers
{
    public static class CustomPager
    {
        public static MvcHtmlString Pager(this HtmlHelper helper, Func<int, string> generateUrl, int currentPage, int totalCount, int pageSize)
        {
            TagBuilder container = new TagBuilder("div");
            container.AddCssClass("pagination-container");
            TagBuilder pagination = new TagBuilder("ul");
            pagination.AddCssClass("pagination");
            int pageCount = totalCount % pageSize > 0 ? totalCount / pageSize + 1 : totalCount / pageSize;

            TagBuilder skipToFirst = new TagBuilder("li");
            skipToFirst.AddCssClass("PagedList-skipToFirst");
            TagBuilder skipToPrevious = new TagBuilder("li");
            skipToPrevious.AddCssClass("PagedList-skipToPrevious");
            TagBuilder aFirst = new TagBuilder("a");
            aFirst.InnerHtml = "««";
            TagBuilder aPrevious = new TagBuilder("a");
            aPrevious.InnerHtml = "«";
            if (currentPage == 1)
            {
                skipToFirst.AddCssClass("disabled");
                skipToPrevious.AddCssClass("disabled");
            }
            else
            {
                
                aFirst.MergeAttribute("href", generateUrl(1));
                aPrevious.MergeAttribute("href", generateUrl(currentPage - 1));
            }
            skipToFirst.InnerHtml += aFirst;
            skipToPrevious.InnerHtml += aPrevious;
            pagination.InnerHtml += skipToFirst;
            pagination.InnerHtml += skipToPrevious;
          
            for (int i = 1; i <= pageCount; i++)
            {
                TagBuilder li = new TagBuilder("li");
                TagBuilder a = new TagBuilder("a");
                a.MergeAttribute("href", generateUrl(i));
                a.InnerHtml += i;
                if (i == currentPage)
                {
                    li.AddCssClass("active");
                }
                li.InnerHtml += a;
                pagination.InnerHtml += li;
            }

            TagBuilder skipToNext = new TagBuilder("li");
            skipToNext.AddCssClass("PagedList-skipToNext");
            TagBuilder skipToLast = new TagBuilder("li");
            skipToLast.AddCssClass("PagedList-skipToLast");
            TagBuilder aNext = new TagBuilder("a");
            aNext.InnerHtml += "»";
            TagBuilder aLast = new TagBuilder("a");
            aLast.InnerHtml += "»»";
            if (currentPage == pageCount)
            {
                skipToNext.AddCssClass("disabled");
                skipToLast.AddCssClass("disabled");
            }
            else
            {
                aNext.MergeAttribute("href", generateUrl(currentPage + 1));
                aLast.MergeAttribute("href", generateUrl(pageCount));
            }
            skipToNext.InnerHtml += aNext;
            skipToLast.InnerHtml += aLast;

            pagination.InnerHtml += skipToNext;
            pagination.InnerHtml += skipToLast;

            container.InnerHtml += pagination;
            return MvcHtmlString.Create(container.ToString());
        }
    }
}