using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Axial.Lang.Enums
{
    public abstract class Enumeration<T> : IEnumerable<Enumeration<T>>, IComparable where T : Enumeration<T>, new()
    {
        private readonly T _value;
        private readonly string _displayName;

        private Dictionary<T, string> _values = new Dictionary<T, string>();

        protected Enumeration()
        {
        }

        protected Enumeration(T value, string displayName)
        {
            _value = value;
            _displayName = displayName;
        }

        public T Value
        {
            get { return _value; }
        }

        public string DisplayName
        {
            get { return _displayName; }
        }

        public override string ToString()
        {
            return DisplayName;
        }

        public override bool Equals(object obj)
        {
            var otherValue = obj as Enumeration<T>;

            if (otherValue == null) {
                return false;
            }

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = _value.Equals(otherValue.Value);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public static Enumeration<T> FromValue(T value)
        {
            var matchingItem = Parse<T,>(value, "value", item => item.Value == value);
            return matchingItem;
        }

        public static T FromDisplayName(string displayName)
        {
            var matchingItem = Parse<T, string>(displayName, "display name", item => item.DisplayName == displayName);
            return matchingItem;
        }

        private static T Parse(T value, string description, Func<T, bool> predicate)
        {
            var type = typeof(Enumeration<T>);

            var emumerator = ((Enumeration<T>)type).GetEnumerator();
            var matchingItem = GetEnumerator().FirstOrDefault(predicate);

            if (matchingItem == null) {
                var message = string.Format("'{0}' is not a valid {1} in {2}", value, description, typeof(T));
                throw new ApplicationException(message);
            }

            return matchingItem;
        }

        public int CompareTo(object other)
        {
            return Value.CompareTo(((Enumeration<T>)other).Value);
        }

        public IEnumerator<Enumeration<T>> GetEnumerator()
        {
            var type = typeof(T);
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            foreach (var info in fields) {
                var instance = new T();
                var locatedValue = info.GetValue(instance);

                if (locatedValue != null) {
                    yield return (Enumeration<T>)locatedValue;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
