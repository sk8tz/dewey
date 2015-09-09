using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Globalization;

namespace Dewey.Net.Dynamic
{
    public static class DynamicExtensions
    {
        public static dynamic ToDynamic(this object value)
        {
            var result = new DynamicDictionary();

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(value.GetType())) {
                result.Add(property.Name, property.GetValue(value));
            }

            return result;
        }
    }
}
