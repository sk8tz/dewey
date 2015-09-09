using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Dewey.Net.Mvc.Html
{
    public static class HtmlExtensions
    {
        public static bool HasError(this HtmlHelper htmlHelper, string key)
        {
            return htmlHelper.ViewData.ModelState[key]?.Errors.Count > 0;
        }

        public static bool HasErrors(this HtmlHelper htmlHelper)
        {
            return htmlHelper.ViewData.ModelState
                .Select(t => t.Value).ToList()
                .Any(v => v.Errors.Count > 0);
        }

        public static List<ModelError> GetErrors(this HtmlHelper htmlHelper)
        {
            return htmlHelper.ViewData.ModelState
                .Select(t => t.Value).ToList()
                .Where(v => v.Errors.Any()).Select(v => v.Errors)
                .SelectMany(e => e).ToList();
        }

        public static IEnumerable<SelectListItem> ToSelectList<T>(this IEnumerable<T> items, Func<T, string> text, Func<T, string> value = null, Func<T, bool> selected = null)
        {
            return items?.Select(p => new SelectListItem
            {
                Text = text.Invoke(p),
                Value = value.Invoke(p) ?? text.Invoke(p),
                Selected = selected?.Invoke(p) ?? false
            }).OrderBy(t => t.Text).ToList();
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey> (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();

            foreach (var element in source) {
                if (seenKeys.Add(keySelector(element))) {
                    yield return element;
                }
            }
        }
    }
}