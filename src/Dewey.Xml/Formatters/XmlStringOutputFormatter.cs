using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNet.Mvc.Formatters;
using Microsoft.AspNet.Mvc.Formatters.Xml.Internal;
using Microsoft.AspNet.Mvc.Internal;
using Microsoft.Net.Http.Headers;

namespace Dewey.Xml.Formatters
{
    /// <summary>
    /// This class handles serialization of objects
    /// to XML using <see cref="XmlSerializer"/>
    /// </summary>
    public class XmlStringOutputFormatter : OutputFormatter
    {
        /// <summary>
        /// Initializes a new instance of <see cref="XmlStringOutputFormatter"/>
        /// with default XmlWriterSettings.
        /// </summary>
        public XmlStringOutputFormatter() :
            this(FormattingUtilities.GetDefaultXmlWriterSettings())
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="XmlStringOutputFormatter"/>
        /// </summary>
        /// <param name="writerSettings">The settings to be used by the <see cref="XmlSerializer"/>.</param>
        public XmlStringOutputFormatter(XmlWriterSettings writerSettings)
        {
            if (writerSettings == null) {
                throw new ArgumentNullException(nameof(writerSettings));
            }

            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);

            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/xml"));

            WriterSettings = writerSettings;
        }

        /// <summary>
        /// Gets the settings to be used by the XmlWriter.
        /// </summary>
        public XmlWriterSettings WriterSettings { get; }

        /// <inheritdoc />
        protected override bool CanWriteType(Type type)
        {
            return true;
        }

        /// <inheritdoc />
        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            if (context == null) {
                throw new ArgumentNullException(nameof(context));
            }

            var response = context.HttpContext.Response;

            var writerSettings = WriterSettings.Clone();
            writerSettings.Encoding = context.ContentType.Encoding ?? Encoding.UTF8;

            // Wrap the object only if there is a wrapping type.
            var value = context.Object;

            using (var textWriter = context.WriterFactory(context.HttpContext.Response.Body, writerSettings.Encoding)) {
                textWriter.Write(value);
            }

            return TaskCache.CompletedTask;
        }
    }
}