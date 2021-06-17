using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestAPIDemo.DB;
using RestAPIDemo.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestAPIDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        [HttpPost]
        [Route("InsertNews")]
        public IActionResult news([FromBody] NewsModel model)
        {
            using (var db = new DbintranetContext())
            {
                if (model != null)
                {
                    news add = new news();
                    add.headNews = model.HeadNew;
                    add.contentShort = model.contentShort;
                    add.contentAll = model.contentAll;
                    add.imagePath = model.imgPath;
                    add.created_by = model.create_by;
                    add.created_date = DateTime.Now;
                    add.depID = model.depID;
                    db.news.Add(add);
                    db.SaveChanges();
                    return Ok("200");
                }
                else
                {
                    return Ok("Error");
                }
            }
        }

        [HttpPost]
        [Route("GetNewsDetail")]
        public IActionResult newHr([FromBody] NewsModel model)
        {
            using (var db = new DbintranetContext())
            {
                var ans = db.news.Where(z => z.id == model.depID).FirstOrDefault();

                return Ok(JsonConvert.SerializeObject(ans));
            }
        }


        [HttpPost]
        [Route("GetNewsForm")]
        public IActionResult HrForm([FromBody] NewsModel model)
        {
            using (var db = new DbintranetContext())
            {
                var ans = db.news.Where(z => z.depID == model.depID).ToList();

                return Ok(JsonConvert.SerializeObject(ans));
            }
        }


        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 409715200)]
        [RequestSizeLimit(409715200)]
        public IActionResult Upload()
        {
            try
            {
                var file = Request.Form.Files[0];

                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

    }
}
