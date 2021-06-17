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
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        // GET: api/<QuestionsController1>
        [HttpGet]
        [Route("GetSelectQuestion")]
        public IEnumerable<questions> GetSelectQuestion()
        {
            using (var db = new DbintranetContext())
            {
                return db.questions.Where(y => y.answers == null).ToList();
            }
        }

        [HttpPost]
        [Route("GetQuestionForDepart")]
        public IActionResult DepQ([FromBody] QuestionsModel model)
        {
            using (var db = new DbintranetContext())
            {
               /* var ans = db.questions.Where(z => z.topicDep == model.depid).ToList();*/

                var res = db.questions.Join(
                  db.employees,
                  questions => questions.created_by,
                  employees => employees.id,
                   (questions, employees) => new
                   {
                       id = questions.id,
                       subject = questions.subject,
                       questions = questions.questions1,
                       answer = questions.answers,
                       created_date = questions.created_date,
                       topicDep = questions.topicDep,
                       name = employees.name,
                       surname = employees.surname
                   }
                  ).Where(z => z.topicDep == model.depid && z.answer == null).ToList();

                return Ok(JsonConvert.SerializeObject(res));
            }
        }

        [HttpPost]
        [Route("GetDepartment")]
        public IActionResult Dep([FromBody] QuestionsModel model)
        {
            using (var db = new DbintranetContext())
            {
                var ans = db.department.ToList();

                return Ok(JsonConvert.SerializeObject(ans));
            }
        }


        [HttpPost]
        [Route("GetQuestion")]
        public IActionResult answer([FromBody] QuestionsModel model)
        {
            using (var db = new DbintranetContext())
            {
               var ans = db.questions.Where(z => z.id == model.ansid).FirstOrDefault(); 

               return Ok(JsonConvert.SerializeObject(ans));
            }
        }

        [HttpPost]
        [Route("UpdateQuestion")]
        public IActionResult answerUpdate([FromBody] QuestionsModel model)
        {
            using (var db = new DbintranetContext())
            {
                if (model != null)
                {
                    var tb = db.questions;
                    var dataupdate = db.questions.Where(z => z.id == model.ansid).FirstOrDefault(); //เอาแค่ แถวเดียวเท่านั้น
                    dataupdate.answers = model.answer;
                    dataupdate.updated_by = model.update_by;
                    dataupdate.update_date = DateTime.Now;
                    tb.Update(dataupdate);
                    db.SaveChanges();

                    return Ok("200");
                }
                else
                {
                    return Ok(null);
                }
            }
        }

        [HttpPost]
        [Route("GetSuccessQuestion")]
        
            public IActionResult Questions([FromBody] QuestionsModel model)
            {
                using (var db = new DbintranetContext())
                {

                var res = db.questions.Join(
                        db.register,
                        questions => questions.created_by,
                        register => register.id,
                         (questions, register) => new
                         {
                             subject = questions.subject,
                             question = questions.questions1,
                             answer = questions.answers,
                             create_date = questions.created_date,
                             create_by = questions.created_by,
                             updated_date = questions.update_date
                         }
                        )
                       .Where(
                       x => x.answer != null).ToList()
                       .OrderByDescending(s => s.updated_date);

                    return Ok(JsonConvert.SerializeObject(res));
                }
            }

        [HttpPost]
        [Route("GetPendingQuestion")]

        public IActionResult QuestionPen([FromBody] QuestionsModel model)
        {
            using (var db = new DbintranetContext())
            {

                var res = db.questions.Join(
                        db.register,
                        questions => questions.created_by,
                        register => register.id,
                         (questions, register) => new
                         {
                             subject = questions.subject,
                             question = questions.questions1,
                             answer = questions.answers,
                             create_date = questions.created_date,
                             create_by = questions.created_by
                         }
                        )
                       .Where(
                       x => x.answer == null && x.create_by == model.create_by).ToList()
                       .OrderByDescending(s => s.create_date);

                return Ok(JsonConvert.SerializeObject(res));
            }
        }

        [HttpPost]
        [Route("GetMyQuestion")]

        public IActionResult MyQuestions([FromBody] QuestionsModel model)
        {
            using (var db = new DbintranetContext())
            {

                var res = db.questions.Join(
                        db.register,
                        questions => questions.created_by,
                        register => register.id,
                         (questions, register) => new
                         {
                             subject = questions.subject,
                             question = questions.questions1,
                             answer = questions.answers,
                             create_date = questions.created_date,
                             create_by = questions.created_by,
                             updated_date = questions.update_date
                         }
                        )
                       .Where(
                       x => x.create_by == model.create_by && x.answer != null).ToList()
                       .OrderByDescending(s => s.updated_date);

                return Ok(JsonConvert.SerializeObject(res));
            }
        }

        [HttpPost]
        [Route("InsertQuestion")]
        public IActionResult Question([FromBody] QuestionsModel model)
        {
            using (var db = new DbintranetContext())
            {
                if (model != null)
                {
                    questions add = new questions();
                    add.subject = model.topic;
                    add.questions1 = model.question;
                    add.created_by = model.create_by;
                    add.created_date = DateTime.Now;
                    add.topicDep = model.depid;
                    db.questions.Add(add);
                    db.SaveChanges();
                    return Ok("200");
                }
                else
                {
                    return Ok("Error");
                }
            }
        }

        }
}
