using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using RestAPIDemo.Model;
using System.IO;
using RestAPIDemo.DB;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Data;

namespace RestAPIDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {

        [HttpPost]
        [Route("GetTopicGallary")]
        public IActionResult topicGall()
        {
            using (var db = new DbintranetContext())
            {
                var ans = db.topicGallary.ToList();

                return Ok(JsonConvert.SerializeObject(ans));
            }
        }


        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 409715200)]
        [RequestSizeLimit(409715200)]
        public IActionResult UploadImage()
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
                    int id_img;
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    using (var db = new DbintranetContext())
                    {
                        
                        galleryMain add = new galleryMain();
                        add.imgMain_Path = dbPath;
                        db.galleryMain.Add(add);
                        db.SaveChanges();

                        id_img = add.id;
                    }
                    return Ok(new { dbPath, id_img });
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



        private static IWebHostEnvironment _webHostEnvironment;

        private int id_imgSub;
        public ImagesController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        [Route ("uploadMultiple")]
        public IActionResult UploadFile(IFormFile[] files)
        {
            int count = files.Length;
            int[] terms = new int[count];
            
            foreach (var file in files)
            {
                

                if (!Directory.Exists(_webHostEnvironment.WebRootPath + "\\Image\\"))
                {
                    Directory.CreateDirectory(_webHostEnvironment.WebRootPath + "\\Image\\");
                }

                using (FileStream filestream = System.IO.File.Create(_webHostEnvironment.WebRootPath + "\\Image\\" + file.FileName))
                {
                    file.CopyTo(filestream);
                }
            }

            using (var db = new DbintranetContext())
            {
                for (var i = 0; i < files.Length; i++)
                {
                    gallerySub add = new gallerySub();
                    add.imgSub_Path = "https://localhost:44335/Image/" + files[i].FileName;
                    add.created_date = DateTime.Now;
                    db.gallerySub.Add(add);
                    db.SaveChanges();
                    terms [i] = add.id;
                }
            }
            return Ok(new { terms });
        }


        [HttpPost]
        [Route("UpdateImageMain")]
        public IActionResult imgUpMain([FromBody] ImagesModel model)
        {
            using (var db = new DbintranetContext())
            {
                if (model != null)
                {
                    //update
                    var tb = db.galleryMain;
                    var dataupdate = db.galleryMain.Where(x => x.id == model.Id).FirstOrDefault(); //เอาแค่ แถวเดียวเท่านั้น
                    dataupdate.topicType = model.topicType;
                    dataupdate.topicDetail = model.topicDetail;
                    dataupdate.created_by = model.created_by;
                    dataupdate.created_date = DateTime.Now;
                    tb.Update(dataupdate);
                    db.SaveChanges();

                    return Ok(200);
                }
                else
                {
                    return Ok("Error");
                }
            }
        }

        [HttpPost]
        [Route("GetGallarySub")]
        public IActionResult GallSub([FromBody] GetDataImageModel model)
        {
            using (var db = new DbintranetContext())
            {
                var ans = db.galleryMain.Where(x => x.topicType == model.topicType).ToList();

                return Ok(JsonConvert.SerializeObject(ans));
            }
        }

        [HttpPost]
        [Route("GetGallarySubAll")]
        public IActionResult GallSubAll([FromBody] GetDataImageModel model)
        {
            using (var db = new DbintranetContext())
            {
                var ans = db.gallerySub.Where(x => x.imgMain_ID == model.MainID).ToList();

                return Ok(JsonConvert.SerializeObject(ans));
            }
        }

        [HttpPost]
        [Route("InsertImageSub")]
        public IActionResult imgUpSub([FromBody] ImagesModel model)
        {
            using (var db = new DbintranetContext())
            {
                
                if (model != null)
                {
                    for(var i = 0;i < model.idimg.Length; i++)
                    {
                        var tb = db.gallerySub;
                        var dataupdate = db.gallerySub.Where(x => x.id == model.idimg[i]).FirstOrDefault(); //เอาแค่ แถวเดียวเท่านั้น
                        dataupdate.imgMain_ID = model.Id;
                        dataupdate.created_by = model.created_by;
                        tb.Update(dataupdate);
                        db.SaveChanges();
                    }
                    return Ok("200");
                }
                else
                {
                    return Ok("Error");
                }
            }
        }


        [HttpPost]
        [Route("GetTopicGallaryHR")]
        public IActionResult topicGallHR([FromBody] GetDataImageModel model)
        {
            using (var db = new DbintranetContext())
            {
                var ans = db.topicGallary.Where(x => x.depID == model.MainID).ToList();

                return Ok(JsonConvert.SerializeObject(ans));
            }
        }
    }
}
