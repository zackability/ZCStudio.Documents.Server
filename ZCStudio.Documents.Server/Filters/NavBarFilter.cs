﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using ZCStudio.Documents.Server.Models;

namespace ZCStudio.Documents.Server.Filters
{
    public class NavBarFilter : IActionFilter
    {
        public List<DocCard> docCards;

        public NavBarFilter(IOptionsSnapshot<List<DocCard>> docCardsOptionsAccessor)
        {
            docCards = docCardsOptionsAccessor.Value;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            ((Controller)context.Controller).ViewBag.DocCards = docCards.Where(i => !string.IsNullOrEmpty(i.DocName)).Take(3);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }
}