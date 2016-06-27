﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace Dewey.WinForms
{
    public class SortableBindingList<T> : BindingList<T>
    {
        private static readonly Dictionary<string, Func<List<T>, IEnumerable<T>>> _cachedOrderByExpressions = new Dictionary<string, Func<List<T>, IEnumerable<T>>>();
        private List<T> _originalList;

        private readonly Action<SortableBindingList<T>, List<T>> _populateBaseList = (a, b) => a.ResetItems(b);

        private ListSortDirection _sortDirection;
        private PropertyDescriptor _sortProperty;

        public SortableBindingList()
        {
            _originalList = new List<T>();
        }

        public SortableBindingList(IEnumerable<T> enumerable)
        {
            if (enumerable != null) {
                _originalList = enumerable.ToList();
            }
            _populateBaseList(this, _originalList);
        }

        public SortableBindingList(List<T> list)
        {
            _originalList = list;
            _populateBaseList(this, _originalList);
        }

        protected override bool SupportsSortingCore { get { return true; } }

        protected override ListSortDirection SortDirectionCore { get { return _sortDirection; } }

        protected override PropertyDescriptor SortPropertyCore { get { return _sortProperty; } }

        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            _sortProperty = prop;

            var orderByMethodName = (_sortDirection == ListSortDirection.Ascending) ? "OrderBy" : "OrderByDescending";
            var cacheKey = typeof(T).GUID + prop.Name + orderByMethodName;

            if (!_cachedOrderByExpressions.ContainsKey(cacheKey)) {
                CreateOrderByMethod(prop, orderByMethodName, cacheKey);
            }

            ResetItems(_cachedOrderByExpressions[cacheKey](_originalList).ToList());
            ResetBindings();
            _sortDirection = _sortDirection == ListSortDirection.Ascending ?
                ListSortDirection.Descending : ListSortDirection.Ascending;
        }

        private void CreateOrderByMethod(PropertyDescriptor prop, string orderByMethodName, string cacheKey)
        {
            var sourceParameter = Expression.Parameter(typeof(List<T>), "source");
            var lambdaParameter = Expression.Parameter(typeof(T), "lambdaParameter");
            var accesedMember = typeof(T).GetProperty(prop.Name);
            var propertySelectorLambda =
                Expression.Lambda(Expression.MakeMemberAccess(lambdaParameter,
                    accesedMember), lambdaParameter);
            var orderByMethod = typeof(Enumerable).GetMethods()
                                                   .Where(a => a.Name == orderByMethodName &&
                                                               a.GetParameters().Length == 2)
                                                   .Single()
                                                   .MakeGenericMethod(typeof(T), prop.PropertyType);

            var orderByExpression = Expression.Lambda<Func<List<T>, IEnumerable<T>>>(
                Expression.Call(orderByMethod,
                    new Expression[] {
                        sourceParameter,
                        propertySelectorLambda
                    }),
                sourceParameter);

            _cachedOrderByExpressions.Add(cacheKey, orderByExpression.Compile());
        }

        protected override void RemoveSortCore()
        {
            ResetItems(_originalList);
        }

        private void ResetItems(List<T> items)
        {
            ClearItems();

            if (items != null) {
                for (var i = 0; i < items.Count; i++) {
                    InsertItem(i, items[i]);
                }
            }
        }

        protected override void OnListChanged(ListChangedEventArgs e)
        {
            _originalList = Items.ToList();
        }
    }
}
