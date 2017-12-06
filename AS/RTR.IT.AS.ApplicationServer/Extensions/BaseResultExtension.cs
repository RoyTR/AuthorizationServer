using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace RTR.IT.AS.ApplicationServer.Extensions
{
    internal static class BaseResultExtension
    {
        internal static string SerializarListToXml<T>(this List<T> listaParametero)
        {
            string resultado;
            var serializer = new XmlSerializer(listaParametero.GetType());
            var settings = new XmlWriterSettings
            {
                Encoding = new UnicodeEncoding(false, false),
                Indent = false,
                OmitXmlDeclaration = false
            };

            using (var textWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(textWriter, settings))
                {
                    serializer.Serialize(xmlWriter, listaParametero);
                }

                resultado = textWriter.ToString();
            }

            return resultado;
        }
    }
}
