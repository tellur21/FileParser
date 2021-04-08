using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace FileParser.Common
{
    public class XmlHandler : IXmlHandler
    { 
        public string ConvertToJson(string inputString)
        {           
            var xml = XElement.Parse(inputString);

            string json = JsonConvert.SerializeXNode(xml, Newtonsoft.Json.Formatting.None, true);

            return json;
        }

        public string ConvertToProtobuf(string inputString)
        {
            throw new NotImplementedException();
        }

        public bool IsValidXml(Stream inputStream)
        {
            try
            {
                new XmlDocument().Load(inputStream);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
