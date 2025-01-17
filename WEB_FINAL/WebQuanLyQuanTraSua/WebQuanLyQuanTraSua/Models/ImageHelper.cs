﻿using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebQuanLyQuanTraSua.Models
{
    public static class cImageHelper
    {
        public static MvcHtmlString Image(this HtmlHelper helper, string src, string altText, string height, string width)
        {
            var builder = new TagBuilder("img");
            builder.MergeAttribute("src", src);
            builder.MergeAttribute("alt", altText);
            builder.MergeAttribute("height", height);
            builder.MergeAttribute("width", width);
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }
    }
}