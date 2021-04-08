using FileParser.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileParser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileParserController : ControllerBase
    {
        private readonly ILogger<FileParserController> _logger;
        private readonly IXmlHandler _xmlHandler;
        private readonly IJsonHandler _jsonHandler;
        private readonly IEmailSender _emailSender;

        public FileParserController(ILogger<FileParserController> logger, IXmlHandler xmlHandler, IJsonHandler jsonHandler, IEmailSender emailSender)
        {
            _logger = logger;
            _xmlHandler = xmlHandler;
            _jsonHandler = jsonHandler;
            _emailSender = emailSender;
        }

        [HttpPost, Route("ConvertStreamToJson")]
        public async Task<IActionResult> ConvertStreamToJson()
        {
            var requestBody = Request.Body;

            using (StreamReader reader = new StreamReader(requestBody))
            {
                var inputString = await reader.ReadToEndAsync();

                var inputStream = inputString.GenerateStreamFromString();

               //dalo by se to optimalizovat pomoci ChainOfResponsibility nebo enum.
               // aby se zjistitlo co za soubor jsme dostaly.

                var isXmlValid = _xmlHandler.IsValidXml(inputStream);
                if (isXmlValid == true)
                {
                    var jsonString = _xmlHandler.ConvertToJson(inputString);

                    var jsonStream = jsonString.GenerateStreamFromString();

                    return File(jsonStream, "application/json");
                }
            }

            return Content("Not valid string to convert to Json");
        }

        [HttpPost, Route("ConvertFileToJsonAndSenByEmail")]
        public async Task<IActionResult> ConvertFileToJsonAndSenByEmail()
        {
            var requestBody = Request.Body;

            using (StreamReader reader = new StreamReader(requestBody))
            {
                var inputString = await reader.ReadToEndAsync();

                var inputStream = inputString.GenerateStreamFromString();

                var isXmlValid = _xmlHandler.IsValidXml(inputStream);
                if (isXmlValid == true)
                {
                    var jsonString = _xmlHandler.ConvertToJson(inputString);

                    var jsonStream = jsonString.GenerateStreamFromString();

                    // Pouze nastrel, neni otestovano
                    await _emailSender.SendEmailAsync(jsonStream);

                    return Ok();
                }
            }

            return Content("Not valid string to convert to Json");
        }

        [HttpPost, Route("ConvertStreamToXml")]
        public async Task<IActionResult> ConvertStreamToXml()
        {
            var requestBody = Request.Body;

            using (StreamReader reader = new StreamReader(requestBody))
            {
                var inputString = await reader.ReadToEndAsync();

                var isJsonValid = _jsonHandler.IsValidJson(inputString);
                if (isJsonValid == true)
                {
                    var xmlString = _jsonHandler.ConvertToXml(inputString);

                    var xmlStream = xmlString.GenerateStreamFromString();

                    return File(xmlStream, "application/json");
                }
            }

            return Content("Not valid file to convert to Json");
        }

        [HttpGet, Route("GetSampleXml")]
        public IActionResult GetSampleXml()
        {
            var file = Path.Combine(Directory.GetCurrentDirectory(), "Files", "sample2.xml");

            var stream = System.IO.File.OpenRead(file);
            return new FileStreamResult(stream, "application/octet-stream");        
        }

        [HttpGet, Route("GetSampleJson")]
        public IActionResult GetSampleJson()
        {
            var file = Path.Combine(Directory.GetCurrentDirectory(), "Files", "note.json");

            var stream = System.IO.File.OpenRead(file);
            return new FileStreamResult(stream, "application/octet-stream");
        }
    }
}
