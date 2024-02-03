using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Buffers.Text;
using System.Net.Mime;
using WebApi.Models;
using static System.Net.Mime.MediaTypeNames;
using ExcelToCsv;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Workbook : ControllerBase
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public Workbook(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }
        [HttpPost]
        [Consumes("application/x-www-form-urlencoded", "application/json")]
        [DisableRequestSizeLimit]
        //[ValidateAntiForgeryToken]
        //[Consumes("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
        public void PostWorkbook([FromForm]WorkbookContent workbookContent)
        {

            var fileContents = Convert.FromBase64String(workbookContent.Base64);
            using (MemoryStream memory = new MemoryStream(fileContents))
                ExcelFileHelper.SaveAsCsv((Stream)memory);
            //System.IO.File.WriteAllBytes(webHostEnvironment.WebRootPath+"\\Book.xlsx", fileContents);
            //return File(fileContents, workbookContent.ContentType, workbookContent.Filename);
        }

    }
}
