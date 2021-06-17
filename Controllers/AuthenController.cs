using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestAPIDemo.DB;
using RestAPIDemo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestAPIDemo.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    /*[Route("[controller]")]*/

    public class AuthenController : ControllerBase
    {
        [HttpGet]
        [Route("GetAllUser")]
        public IEnumerable<register> GetAllUser()
        {
            using (var context = new DbintranetContext())
            {
                //get all registers
                return context.register.ToList();
            }
        }

        // POST api/<AuthenController>
        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            if (model == null)
            {
                return Ok("data not found");
            }
            using (var db = new DbintranetContext())
            {
                //insert
                /*register add = new register();
                add.username = "Demo1";
                add.password = "Pwd1";
                db.register.Add(add);
                db.SaveChanges();*/

                //update
                /* var tb = db.register;
                 var dataupdate = db.register.Where(x => x.username == model.Username).FirstOrDefault(); //เอาแค่ แถวเดียวเท่านั้น
                 dataupdate.password = "NewUpdate";
                 tb.Update(dataupdate);
                 db.SaveChanges();*/

                //delete
                /* var delete = db.register.Where(x => x.username == model.Username).FirstOrDefault();
                 tb.Remove(delete);
                 db.SaveChanges();*/

                //select 1 row
                /*var select1 = db.register.Where(x => x.username == model.Username).FirstOrDefault();*/
                // select multi rows
                /* var select_multirows = db.register.ToList();*/

                // การเช็คข้อมูลใน db
                /* var res = db.register.Where(
                     x => x.username == model.Username
                     && x.password == model.Password)
                     .FirstOrDefault();*/

                var res = db.register.Join(
                    db.employees,
                    register => register.id,
                    employees => employees.regisID,
                     (register, employees) => new
                     {
                         id = register.id,
                         username = register.username,
                         password = register.password,
                         empID = employees.id,
                         depID = employees.depID,
                         name = employees.name,
                         surname = employees.surname,
                         picPath = employees.picPath
                     }
                    )
                   .Where(
                   x => x.username == model.Username
                   && x.password == model.Password)
                   .FirstOrDefault();
                // 
                if (res != null)
                {
                    // login success and return data user
                    return Ok(JsonConvert.SerializeObject(res));
                }
                // login fail return null
                return Ok(null);

            }

        }


        // POST api/<AuthenController>
        /*[HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            var  q =  JsonConvert.SerializeObject(model);
            return Ok(q);
        }*/
    }
}
