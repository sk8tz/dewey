using System.Linq;
using System.Xml.Linq;

namespace Dewey.Xml.Extensions
{
    public static class XElementExtensions
    {
        /// <summary>Gets the first (in document order) child element with the specified <see cref="XName"/>.</summary>
        /// <param name="element">The element.</param>
        /// <param name="name">The <see cref="XName"/> to match.</param>
        /// <param name="ignoreCase">If set to <c>true</c> case will be ignored whilst searching for the <see cref="XElement"/>.</param>
        /// <returns>A <see cref="XElement"/> that matches the specified <see cref="XName"/>, or null. </returns>
        public static XElement Element(this XElement element, XName name, bool ignoreCase)
        {
            var el = element.Element(name);

            if (el != null) {
                return el;
            }

            if (!ignoreCase) {
                return null;
            }

            var elements = element.Elements().Where(e => e.Name.LocalName.ToString().ToLowerInvariant() == name.ToString().ToLowerInvariant());

            return elements.Count() == 0 ? null : elements.First();
        }
    }
}