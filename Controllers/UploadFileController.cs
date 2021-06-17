using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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

namespace RestAPIDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadFileController : ControllerBase
    {
        public string ErrorMessage { get; set; }
        public string ErrorMessageVideo { get; set; }
        public string ErrorMessageVideo2 { get; set; }
        public decimal filesize { get; set; }

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 409715200)]
        [RequestSizeLimit(409715200)]
        public IActionResult Upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                var supportedTypes = new[] { "pdf" };
                var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
                var folderName = Path.Combine("Resources", "Files");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (!supportedTypes.Contains(fileExt))
                {
                    ErrorMessage = "File Extension Is InValid - Only Upload PDF File";
                }
                else
                {
                    if (file.Length > 0)
                    {
                        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        var fullPath = Path.Combine(pathToSave, fileName);
                        var dbPath = Path.Combine(folderName, fileName);

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        return Ok(new { dbPath, fileName, ErrorMessage });
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
               
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
            return Ok(new { ErrorMessage  });
        }

        [HttpPost]
        [Route("UploadVideo")]
        [RequestFormLimits(MultipartBodyLengthLimit = 1073741824)]
        [RequestSizeLimit(1073741824)]
        public IActionResult UploadVideo()
        {
            try
            {
                var file = Request.Form.Files[0];
                var supportedTypes = new[] { "avi", "wmv", "mpeg", "mov", "divx", "dat", "flv", "asf", "mts", "mp4" };
                var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
                var folderName = Path.Combine("Resources", "Videos");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (!supportedTypes.Contains(fileExt))
                {
                    ErrorMessage = "500";
                }
                else if (file.Length > 409715200)
                {
                    ErrorMessage = "413";
                }
                else
                {
                    if (file.Length > 0)
                    {
                        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        var fullPath = Path.Combine(pathToSave, fileName);
                        var dbPath = Path.Combine(folderName, fileName);

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        return Ok(new { dbPath, fileName, ErrorMessage });
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
            return Ok(new { ErrorMessage });
        }

        [HttpPost]
        [Route("InsertFileUpload")]
        public IActionResult Question([FromBody] UploadKMModel model)
        {
            using (var db = new DbintranetContext())
            {
                if (model != null)
                {
                    uploadFileKM add = new uploadFileKM();
                    add.subject_km = model.subject;
                    add.detail_km = model.detail;
                    add.pdfPath = model.pdfPath;
                    add.videoPath = model.videoPath;
                    add.isDelete = false;
                    add.created_by = model.create_by;
                    add.depID = model.dep_id;
                    add.created_date = DateTime.Now;
                    db.uploadFileKM.Add(add);
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
        [Route("GetShowFileKM")]

        public IActionResult ShowFileKM()
        {
            using (var db = new DbintranetContext())
            {

                var res = db.uploadFileKM.Join(
                        db.department,
                        uploadFileKM => uploadFileKM.depID,
                        department => department.id,
                         (uploadFileKM, department) => new
                         {
                             idKM = uploadFileKM.id,
                             subject = uploadFileKM.subject_km,
                             detail = uploadFileKM.detail_km,
                             pdfPath = uploadFileKM.pdfPath,
                             videoPath = uploadFileKM.videoPath,
                             create_date = uploadFileKM.created_date,
                             depName = department.dep_name
                         }
                        )
                       .ToList()
                       .OrderByDescending(s => s.create_date);

                return Ok(JsonConvert.SerializeObject(res));
            }
        }


        [HttpPost]
        [Route("UploadFilesMenu")]
        public IActionResult UploadFileMEne()
        {
            try
            {
                var file = Request.Form.Files[0];
                var supportedTypes = new[] { "pdf" };
                var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
                var folderName = Path.Combine("Resources", "FilesMenu");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (!supportedTypes.Contains(fileExt))
                {
                    ErrorMessage = "File Extension Is InValid - Only Upload PDF File";
                }
                else
                {
                    if (file.Length > 0)
                    {
                        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        var fullPath = Path.Combine(pathToSave, fileName);
                        var dbPath = Path.Combine(folderName, fileName);

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        return Ok(new { dbPath, fileName, ErrorMessage });
                    }
                    else
                    {
                        return BadRequest();
                    }
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
            return Ok(new { ErrorMessage });
        }


        [HttpPost]
        [Route("InsertFileMenu")]
        public IActionResult FileMenu([FromBody] UploadFileMenuModel model)
        {
            using (var db = new DbintranetContext())
            {
                if (model != null)
                {
                    uploadFileMenu add = new uploadFileMenu();
                    add.subject = model.subject;
                    add.URLPath = model.URLPath;
                    add.filePath = model.filePath;
                    add.menuID = model.Id;
                    add.isDelete = 0;
                    add.created_by = model.create_by;
                    add.created_date = DateTime.Now;
                    db.uploadFileMenu.Add(add);
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
        [Route("GetFileMenu")]
        public IActionResult MenuFile([FromBody] UploadFileMenuModel model)
        {
            using (var db = new DbintranetContext())
            {
                var ans = db.uploadFileMenu.Where(x => x.menuID == model.Id).ToList();

                return Ok(JsonConvert.SerializeObject(ans));
            }
        }

        [HttpPost]
        [Route("DeleteKM")]
        public IActionResult DelKM([FromBody] UploadKMModel model)
        {
            using (var db = new DbintranetContext())
            {
                var tb = db.uploadFileKM;
                var delete = db.uploadFileKM.Where(x => x.id == model.id).FirstOrDefault();
                tb.Remove(delete);
                db.SaveChanges();

                return Ok(200);
            }
        }

        [HttpPost]
        [Route("GetDataKM")]
        public IActionResult DataKM([FromBody] UploadKMModel model)
        {
            using (var db = new DbintranetContext())
            {
                var ans = db.uploadFileKM.Where(x => x.id == model.id).ToList();

                return Ok(JsonConvert.SerializeObject(ans));
            }
        }





        /*
                private static IWebHostEnvironment _webHostEnvironment;

                private int id_imgSub;
                public UploadFileController(IWebHostEnvironment webHostEnvironment)
                {
                    _webHostEnvironment = webHostEnvironment;
                }*/

        /*      [HttpPost]
              [Route("UploadVideo")]
              [RequestFormLimits(MultipartBodyLengthLimit = 1073741824)]
              [RequestSizeLimit(1073741824)]
              public IActionResult UploadFile(IFormFile[] files)
              {
                  var supportedTypes = new[] { "mp4","avi","mpeg","avi" };
                  var fileExt = System.IO.Path.GetExtension(files[0].FileName).Substring(1);
                  var filePath =  "\\Videos\\" + files[0].FileName;

                  if (files[0].Length > 409715200)
                  {
                      ErrorMessageVideo2 = "413";
                  }else if (!supportedTypes.Contains(fileExt))
                  {
                      ErrorMessageVideo = "500";
                  }
                  else
                  {

                      foreach (var file in files)
                      {
                          if (!Directory.Exists(_webHostEnvironment.WebRootPath + "\\Videos\\"))
                          {
                              Directory.CreateDirectory(_webHostEnvironment.WebRootPath + "\\Videos\\");
                          }

                          using (FileStream filestream = System.IO.File.Create(_webHostEnvironment.WebRootPath + "\\Videos\\" + file.FileName))
                          {
                              file.CopyTo(filestream);
                          }
                      }
                      return Ok(new { filePath , ErrorMessageVideo, ErrorMessageVideo2 });
                  }
                  return Ok(new { ErrorMessageVideo, ErrorMessageVideo2 });

              }*/



    }
}
