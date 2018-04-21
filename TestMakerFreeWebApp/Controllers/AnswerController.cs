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
    public class AnswerController : BaseApiController
    {
        #region Constructor
        public AnswerController(ApplicationDbContext dbContext) : base(dbContext) { }
        #endregion

        #region RESTful convention methods
        /// <summary>
        /// Retrieves the Answer with the given {id}
        /// </summary>
        /// <param name="id">The Id of an existing Answer</param>
        /// <returns>the Answer with the given {id}</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var answer = base.DbContext.Answers.Where(a => a.Id == id).FirstOrDefault();

            //handle requests asking for non-existing answers
            if(answer == null)
            {
                return NotFound(new { Error = $"AnswerId {id} has not been found" });
            }

            return new JsonResult(answer.Adapt<AnswerViewModel>(), base.JsonSettings);
        }

        /// <summary>
        /// Adds a new Answer to the database
        /// </summary>
        /// <param name="model">The AnswerViewModel containing the data to insert</param>
        [HttpPut]
        public IActionResult Put([FromBody]AnswerViewModel model)
        {
            //return a generic HTTP Status 500(Server Error) if the client payload is invalid
            if(model == null)
            {
                return new StatusCodeResult(500);
            }

            //map the ViewModel to the Model
            var answer = model.Adapt<Answer>();

            //override those properties that should be set from the server-side only
            answer.QuestionId = model.QuestionId;
            answer.Text = model.Text;
            answer.Notes = model.Text;

            //properties set from server-side
            answer.CreatedDate = DateTime.Now;
            answer.LastModifiedDate = answer.CreatedDate;

            //add the new answer
            base.DbContext.Answers.Add(answer);
            base.DbContext.SaveChanges();

            //return the newly created Answer to the client
            return new JsonResult(answer.Adapt<AnswerViewModel>(), base.JsonSettings);
        }

        /// <summary>
        /// Edit the Answer with the given {id}
        /// </summary>
        /// <param name="model">The AnswerViewModel containing the data to update</param>
        [HttpPost]
        public IActionResult Post([FromBody]AnswerViewModel model)
        {
            //return a generic HTTP Status 500 (Server Error) if the client payload is invalid.
            if(model == null)
            {
                return new StatusCodeResult(500);
            }

            //retrieve the answer to edit
            var answer = base.DbContext.Answers.Where(a => a.Id == model.Id).FirstOrDefault();

            //handle requests asking for non-existing answers
            if(answer == null)
            {
                return NotFound(new { Error = $"AnswerId {model.Id} has not been found" });
            }

            //handle the update (without object-mapping) by manually assigning the properties we want to accept from the request
            answer.QuestionId = model.QuestionId;
            answer.Text = model.Text;
            answer.Value = model.Value;
            answer.Notes = model.Notes;

            //properties set from server-side
            answer.LastModifiedDate = DateTime.Now;

            //persist the changes into the database
            base.DbContext.SaveChanges();

            //return the updated Quiz to the client
            return new JsonResult(answer.Adapt<AnswerViewModel>(), base.JsonSettings);
        }

        /// <summary>
        /// Deletes the Answer with the given {id} from the Database
        /// </summary>
        /// <param name="id">The Id of an existing Answer</param>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            //retrieve the answer from the database
            var answer = base.DbContext.Answers.Where(a => a.Id == id).FirstOrDefault();

            //handle requests asking for non-existing answers
            if(answer == null)
            {
                return NotFound(new { Error = $"AnserId {id} has not been found" });
            }

            //remove the answer from the dbContext
            base.DbContext.Answers.Remove(answer);
            base.DbContext.SaveChanges();

            //return an HTTP Status 200(OK)
            return new OkResult();
        }
        #endregion

        #region Attribute-based routing methods
        /// <summary>
        /// GET api/answer/all
        /// </summary>
        /// <param name="questionId"></param>
        /// <returns></returns>
        [HttpGet("All/{questionId}")]
        public IActionResult All(int questionId)
        {
            var answers = base.DbContext.Answers.Where(answer => answer.QuestionId == questionId)
                .ToArray();

            return new JsonResult(answers.Adapt<AnswerViewModel[]>(), base.JsonSettings);
        }
        #endregion
    }
}
