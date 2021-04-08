using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileParser.Common
{
    public interface IJsonHandler
    {
        string ConvertToXml(string inputString);
        string ConvertToProtobuf(string inputString);
        bool IsValidJson(string inputString);
    }
}
