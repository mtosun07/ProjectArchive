using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace TSN.Utility.Extensions
{
    public static class XmlExtensions
    {
        public static IEnumerable<XElement> ElementsNamed(this XmlReader reader, string elementName)
        {
            /* Source: https://stackoverflow.com/a/19165632 */
            reader.MoveToContent();
            reader.Read();
            while (!reader.EOF && reader.ReadState == ReadState.Interactive)
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals(elementName))
                {
                    var matchedElement = XNode.ReadFrom(reader) as XElement;
                    if (matchedElement != null)
                        yield return matchedElement;
                }
                else
                    reader.Read();
            }
        }
        public static void Validate(this XmlDocument xml, XmlSchema xsd)
        {
            if (xml == null)
                throw new ArgumentNullException(nameof(xml));
            if (xsd == null)
                throw new ArgumentNullException(nameof(xsd));
            bool valid = true;
            var exceptions = new List<Exception>(0);
            var messages = new StringCollection();
            void validationCallBack(object sender, ValidationEventArgs e)
            {
                messages.Add(e.Message);
                if (e.Exception != null)
                    exceptions.Add(e.Exception);
                valid = false;
            }
            void throwIfNotValid()
            {
                if (valid)
                    return;
                exceptions.TrimExcess();
                throw new AggregateException($"See inner exceptions for errors. Messages (including warnings too) are listed below if available.{Environment.NewLine}{string.Join(Environment.NewLine, messages)}", exceptions);
            }
            if (!xml.Schemas.Contains(xsd))
                xml.Schemas.Add(xsd);
            if (!xml.Schemas.IsCompiled)
                xml.Schemas.Compile();
            xml.Validate(validationCallBack);
            throwIfNotValid();
        }
        public static void Validate(this XmlDocument xml, Stream xsd, bool dispose = true)
        {
            if (xml == null)
                throw new ArgumentNullException(nameof(xml));
            if (xsd == null)
                throw new ArgumentNullException(nameof(xsd));
            bool valid = true;
            var exceptions = new List<Exception>(0);
            var messages = new StringCollection();
            void validationCallBack(object sender, ValidationEventArgs e)
            {
                messages.Add(e.Message);
                if (e.Exception != null)
                    exceptions.Add(e.Exception);
                valid = false;
            }
            void throwIfNotValid()
            {
                if (valid)
                    return;
                exceptions.TrimExcess();
                throw new AggregateException($"See inner exceptions for errors. Messages (including warnings too) are listed below if available.{Environment.NewLine}{string.Join(Environment.NewLine, messages)}", exceptions);
            }
            try
            {
                var schema = XmlSchema.Read(xsd, validationCallBack);
                throwIfNotValid();
                if (!xml.Schemas.Contains(schema))
                    xml.Schemas.Add(schema);
                if (!xml.Schemas.IsCompiled)
                    xml.Schemas.Compile();
                xml.Validate(validationCallBack);
                throwIfNotValid();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dispose)
                {
                    xsd.Close();
                    xsd.Dispose();
                }
            }
        }
    }
}