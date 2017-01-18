using MovieProjectWithUmbraco.Models;
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
        private const string FOLDER_FILTER_PATH = "~/Views/Partials/Filter/";

        public ActionResult RenderFilterPage(SearchResponse model)
        {
            var filterModel = ReestablishSearchFilter(model);
            return PartialView(FOLDER_FILTER_PATH + "_Filter.cshtml", filterModel);
        }

        private SearchFilter ReestablishSearchFilter(SearchResponse model)
        {
            var filterModel = new SearchFilter
            {
                Types = GetTypesCollection(),
                OrderBy = new Type[] {
                    new Type { IsChecked = true, Name = "CreateDate" },
                    new Type { IsChecked = false, Name = "Name" } }
            };

            RefreshTypesValues(filterModel.Types, GetTypesFromResponse(model.Types));

            ReestablishOrderbyFromResponse(filterModel.OrderBy, model.OrderBy);

            return filterModel;
        }

        private void ReestablishOrderbyFromResponse(IEnumerable<Type> orderByCollection, string orderByString)
        {
            if (orderByString != null)
                RefreshOrderByValues(orderByCollection, orderByString);
            else
                RefreshOrderByValues(orderByCollection, orderByCollection.First().Name);
        }

        private Type[] GetTypesFromResponse(IEnumerable<string> types)
        {
            return types != null 
                ? types.Select(p => new Type { IsChecked = true, Name = p }).ToArray() 
                : new List<Type>().ToArray();
        }

        private Type[] GetTypesCollection()
        {
            var section = (IndexSets)ConfigurationManager.GetSection("ExamineLuceneIndexSets");

            return section.Sets["MySearch"].IncludeNodeTypes.ToList().Select(p => new Type
            {
                IsChecked = true,
                Name = p.Name
            })
            .ToArray();
        }

        private void RefreshOrderByValues(IEnumerable<Type> collection, string param)
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

        private void RefreshTypesValues(IEnumerable<Type> collection, Type[] param)
        {
            foreach (var value in collection)
            {
                if (param.Length == 0 || param.Any(p => p.Name == value.Name))
                {
                    value.IsChecked = true;
                    continue;
                }

                value.IsChecked = false;
            }
        }
    }
}