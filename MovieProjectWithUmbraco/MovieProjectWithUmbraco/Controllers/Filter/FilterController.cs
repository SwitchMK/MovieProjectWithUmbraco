using MovieProjectWithUmbraco.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using System.Linq.Dynamic;
using Examine.LuceneEngine.Config;

namespace MovieProjectWithUmbraco.Controllers.Filter
{
    public class FilterController : SurfaceController
    {
        public ActionResult RenderFilterPage()
        {
            var filterModel = ReestablishSearchFilter();
            return PartialView("~/Views/Partials/Filter/_Filter.cshtml", filterModel);
        }

        private SearchFilter ReestablishSearchFilter()
        {
            var queryString = Request.QueryString["query"];

            var filterModel = new SearchFilter
            {
                Types = GetTypesCollection(),
                OrderBy = new Models.Type[] { new Models.Type { IsChecked = true, Name = "CreateDate" }, new Models.Type { IsChecked = false, Name = "Name" } },
                Query = queryString
            };

            RefreshTypesValues(filterModel.Types, GetTypesFromResponse());

            ReestablishOrderbyFromResponse(filterModel.OrderBy);

            return filterModel;
        }

        private void ReestablishOrderbyFromResponse(IEnumerable<Models.Type> orderByCollection)
        {
            var orderByString = Request.QueryString["orderby"];

            if (orderByString != null)
                RefreshOrderByValues(orderByCollection, orderByString);
            else
                RefreshOrderByValues(orderByCollection, orderByCollection.First().Name);
        }

        private Models.Type[] GetTypesFromResponse()
        {
            var typesString = Request.QueryString["types"];
            var types = typesString != null ? typesString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList() : new List<string>();

            return types.Select(p => new Models.Type { IsChecked = true, Name = p }).ToArray();
        }

        private Models.Type[] GetTypesCollection()
        {
            var section = ConfigurationManager.GetSection("ExamineLuceneIndexSets") as IndexSets;

            return section.Sets["MySearch"].IncludeNodeTypes.ToList().Select(p => new Models.Type
            {
                IsChecked = false,
                Name = p.Name
            })
            .ToArray();
        }

        private void RefreshOrderByValues(IEnumerable<Models.Type> collection, string param)
        {
            foreach (var value in collection)
            {
                if (value.Name == param)
                {
                    value.IsChecked = true;
                    continue;
                }

                value.IsChecked = false;
            }
        }

        private void RefreshTypesValues(IEnumerable<Models.Type> collection, Models.Type[] param)
        {
            foreach (var value in collection)
            {
                if (param.Any(p => p.Name == value.Name))
                {
                    value.IsChecked = true;
                    continue;
                }

                value.IsChecked = false;
            }
        }
    }
}