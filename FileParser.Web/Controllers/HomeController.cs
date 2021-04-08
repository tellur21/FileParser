using FileParser.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FileParser.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ConvertFileToJson(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Content("file not selected");

            var uri = new Uri("https://localhost:5001/api/FileParser/ConvertStreamToJson");
            var fileName = Path.GetFileNameWithoutExtension(file.FileName);

            using (var stream = file.OpenReadStream())
            {
                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync(uri, new StreamContent(stream));
                    if (response.IsSuccessStatusCode)
                    {
                        var reponceStream = await response.Content.ReadAsStreamAsync();

                        var mimeType = "text/plain";

                        return new FileStreamResult(reponceStream, mimeType)
                        {
                            FileDownloadName = $"{fileName}.json"
                        };
                    }
                    else //web api sent error response 
                    {
                        return BadRequest();
                    }
                }
            }

        }

        [HttpPost]
        public async Task<IActionResult> ConvertFileToJsonAndSenByEmail(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Content("file not selected");

            var uri = new Uri("https://localhost:5001/api/FileParser/ConvertFileToJsonAndSenByEmail");
            var fileName = Path.GetFileNameWithoutExtension(file.FileName);

            using (var stream = file.OpenReadStream())
            {
                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync(uri, new StreamContent(stream));
                    if (response.IsSuccessStatusCode)
                    {
                        return Ok();
                    }
                    else //web api sent error response 
                    {
                        return BadRequest();
                    }
                }
            }

        }

        [HttpPost]
        public async Task<IActionResult> ConvertFileToXml(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Content("file not selected");

            var uri = new Uri("https://localhost:5001/api/FileParser/ConvertStreamToXml");
            var fileName = Path.GetFileNameWithoutExtension(file.FileName);

            using (var stream = file.OpenReadStream())
            {
                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync(uri, new StreamContent(stream));
                    if (response.IsSuccessStatusCode)
                    {
                        var reponceStream = await response.Content.ReadAsStreamAsync();

                        var mimeType = "text/plain";

                        return new FileStreamResult(reponceStream, mimeType)
                        {
                            FileDownloadName = $"{fileName}.xml"
                        };
                    }
                    else //web api sent error response 
                    {
                        return BadRequest();
                    }
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> ConvertToXmlFromUri(string url)
        {
            if (string.IsNullOrEmpty(url))
                return Content("url is empty");

            var uri = new Uri("https://localhost:5001/api/FileParser/ConvertStreamToXml");

            using (var client = new HttpClient())
            {
                using (var result = await client.GetAsync(url))
                {
                    if (result.IsSuccessStatusCode)
                    {
                        using (var stream = await result.Content.ReadAsStreamAsync())
                        {
                            var response = await client.PostAsync(uri, new StreamContent(stream));
                            if (response.IsSuccessStatusCode)
                            {
                                var reponceStream = await response.Content.ReadAsStreamAsync();

                                var mimeType = "text/plain";

                                return new FileStreamResult(reponceStream, mimeType)
                                {
                                    FileDownloadName = $"test.xml"
                                };
                            }
                            else //web api sent error response 
                            {
                                return BadRequest();
                            }
                        }
                    }
                    else //web api sent error response 
                    {
                        return BadRequest();
                    }
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> ConvertToJsonFromUri(string url)
        {
            if (string.IsNullOrEmpty(url))
                return Content("url is empty");

            var uri = new Uri("https://localhost:5001/api/FileParser/ConvertStreamToJson");

            using (var client = new HttpClient())
            {
                using (var result = await client.GetAsync(url))
                {
                    if (result.IsSuccessStatusCode)
                    {
                        using (var stream = await result.Content.ReadAsStreamAsync())
                        {
                            var response = await client.PostAsync(uri, new StreamContent(stream));
                            if (response.IsSuccessStatusCode)
                            {
                                var reponceStream = await response.Content.ReadAsStreamAsync();

                                var mimeType = "text/plain";

                                return new FileStreamResult(reponceStream, mimeType)
                                {
                                    FileDownloadName = $"test.json"
                                };
                            }
                            else //web api sent error response 
                            {
                                return BadRequest();
                            }
                        }
                    }
                    else //web api sent error response 
                    {
                        return BadRequest();
                    }
                }
            }
        }
    }
}
