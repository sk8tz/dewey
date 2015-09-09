using Dewey.Net.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Dewey.Net.Enums
{
    public static class HEnumExtensions<T>
    {
        public static IList<T> Values
        {
            get
            {
                var type = typeof(T);

                var enumValues = new List<T>();

                foreach (FieldInfo fi in type.GetFields(BindingFlags.Static | BindingFlags.Public)) {
                    enumValues.Add((T)Enum.Parse(type, fi.Name, false));
                }

                return enumValues;
            }
        }
    }

    public static class EnumExtensions
    {
        public static FieldInfo[] GetPublicStaticFields<T>(this T reference)
        {
            var type = typeof(T);

            return type.GetFields(BindingFlags.Static | BindingFlags.Public);
        }

        public static IList<T> GetNames<T>(this T reference)
        {
            var type = typeof(T);

            var enumValues = new List<T>();

            foreach (FieldInfo fi in type.GetFields(BindingFlags.Static | BindingFlags.Public)) {
                enumValues.Add((T)Enum.Parse(type, fi.Name, false));
            }

            return enumValues;
        }

        public static string GetName<T>(this T reference)
        {
            return reference.ToString();
        }

        public static IList GetValues<T>(this T reference)
        {
            var type = typeof(T);

            var enumValues = new List<T>();

            foreach (FieldInfo fi in type.GetFields(BindingFlags.Static | BindingFlags.Public)) {
                enumValues.Add((T)Enum.Parse(type, fi.Name, false));
            }

            return enumValues;
        }

        public static IList<string> GetDescriptions<T>(this T reference)
        {
            var fields = reference.GetPublicStaticFields();

            return fields
                .Select(t => t.GetDescription(t.Name))
                .ToList();
        }

        public static string GetDescription<T>(this T reference, string name)
        {
            var fields = reference.GetPublicStaticFields();

            var descriptionAttributes = fields
                .Where(t => t.Name == name)
                .FirstOrDefault()?
                .GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];

            if (descriptionAttributes == null) {
                return string.Empty;
            }

            return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Name : name.AsDisplayName();
        }

        public static T GetByName<T>(string name)
        {
            return (T)Enum.Parse(typeof(T), name, true);
        }

        public static T GetByDescription<T>(string description)
        {
            var type = typeof(T);

            foreach (var field in type.GetFields()) {
                var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

                if (attribute != null) {
                    if (attribute.Description == description) {
                        return (T)field.GetValue(null);
                    }
                } else {
                    if (field.Name == description) {
                        return (T)field.GetValue(null);
                    }
                }
            }

            throw new ArgumentException("Not found.", "description");
        }
    }
}
