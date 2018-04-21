using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TestMakerFreeWebApp.Data;
using TestMakerFreeWebApp.Data.Models;
using TestMakerFreeWebApp.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestMakerFreeWebApp.Controllers
{
    public class QuestionController : BaseApiController
    {
        #region Constructor
        public QuestionController(ApplicationDbContext dbContext) : base(dbContext) { }
        #endregion

        #region RESTful convention methods
        /// <summary>
        /// Retrieves the Question with the given {id}
        /// </summary>
        /// <param name="id">The Id of an existing Question</param>
        /// <returns>the Question with the given {id}</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var question = base.DbContext.Questions.Where(q => q.Id == id)
                .FirstOrDefault();

            //handle requests asking for non-existing questions
            if (question == null)
            {
                return NotFound(new { Error = $"QuestionId {id} has not been found" });
            }

            return new JsonResult(question.Adapt<QuestionViewModel>(), base.JsonSettings);
        }

        /// <summary>
        /// Adds a new Question to the database
        /// </summary>
        /// <param name="model">The QuestionViewModel containing the data to insert</param>
        [HttpPut]
        public IActionResult Put([FromBody] QuestionViewModel model)
        {
            //return a generic HTTP status 500 (server error) if the client payload is invalid
            if(model == null)
            {
                return new StatusCodeResult(500);
            }

            //map the ViewModel to the Model
            var question = model.Adapt<Question>();

            //override those properties that should be set from the server-side only
            question.QuizId = model.QuizId;
            question.Text = model.Text;
            question.Notes = model.Notes;

            //properties set from server-side
            question.CreatedDate = DateTime.Now;
            question.LastModifiedDate = question.CreatedDate;

            //add the new question
            base.DbContext.Questions.Add(question);
            base.DbContext.SaveChanges();

            return new JsonResult(question.Adapt<QuestionViewModel>(), base.JsonSettings);
        }

        /// <summary>
        /// Edit the Question with the given {id}
        /// </summary>
        /// <param name="model">The QuestionViewModel containing the data to update</param>
        [HttpPost]
        public IActionResult Post(QuestionViewModel model)
        {
            //return a generic HTTP Status 500 (server Error) if the client payload is invalid
            if(model == null)
            {
                return new StatusCodeResult(500);
            }

            //retrieve the question to edit
            var question = base.DbContext.Questions.Where(q => q.Id == model.Id).FirstOrDefault();

            //handle requests asking for non-existing questions
            if(question == null)
            {
                return NotFound(new { Error = $"QuestionId {model.Id} has not been found" });
            }

            //handle the update (without object-mapping) by manually assinging the properties we want to accept from the request
            question.QuizId = model.QuizId;
            question.Text = model.Text;
            question.Notes = model.Notes;

            //properties set from server-side
            question.LastModifiedDate = DateTime.Now;

            //persist the changes into the database
            base.DbContext.SaveChanges();

            return new JsonResult(question.Adapt<QuestionViewModel>(), base.JsonSettings);
        }

        /// <summary>
        /// Deletes the Question with the given {id} from the Database
        /// </summary>
        /// <param name="id">The Id of an existing Question</param>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            //retrieve the quesion from the database
            var question = base.DbContext.Questions.Where(q => q.Id == id).FirstOrDefault();

            //handle requestion asking for non-existing questions
            if(question == null)
            {
                return NotFound(new { Error = $"QuestionId {id} has not been found" });
            }

            //remove the quiz from the dbContext
            base.DbContext.Questions.Remove(question);
            base.DbContext.SaveChanges();

            //return an HTTP Status 200 (OK)
            return new OkResult();
        }
        #endregion

        #region Attribute-based routing methods
        /// <summary>
        /// GET api/question/all
        /// </summary>
        /// <param name="quizId"></param>
        /// <returns></returns>
        [HttpGet("All/{quizId}")]
        public IActionResult All(int quizId)
        {
            var questions = base.DbContext.Questions.Where(q => q.QuizId == quizId).ToArray();

            return new JsonResult(questions.Adapt<QuestionViewModel[]>(), base.JsonSettings);
        }
        #endregion
    }
}
