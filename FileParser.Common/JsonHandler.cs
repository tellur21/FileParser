using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace FileParser.Common
{
    public class JsonHandler : IJsonHandler
    {
        public virtual string ConvertToXml(string inputString)        
        {          
            var xml = JsonConvert.DeserializeXmlNode(inputString, "root");

            var xDoc = XDocument.Parse(xml.InnerXml);

            var xmlString = xDoc.ToString();

            return xmlString;
        }

        public virtual string ConvertToProtobuf(string inputString)
        {
            throw new NotImplementedException();
        }

        public virtual bool IsValidJson(string inputString)
        {
            try
            {
                JToken.Parse(inputString);
                return true;
            }
            catch
            {                
                return false;
            }
        }
    }
}
