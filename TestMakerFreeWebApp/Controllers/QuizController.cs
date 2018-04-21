using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TestMakerFreeWebApp.Data;
using TestMakerFreeWebApp.ViewModels;
using Mapster;
using TestMakerFreeWebApp.Data.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestMakerFreeWebApp.Controllers
{
    public class QuizController : BaseApiController
    {

        #region Constructor
        public QuizController(ApplicationDbContext dbContext) : base(dbContext) { }
        #endregion

        #region RESTful conventions methods
        /// <summary>
        /// GET: api/quiz/{id}
        /// Retrieves the Quiz with the given {id}
        /// </summary>
        /// <param name="id">The ID of an existing Quiz</param>
        /// <returns>the Quiz with the given {id}</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var quiz = base.DbContext.Quizzes.Where(q => q.Id == id).FirstOrDefault();

            //handle requests asking for non-existing quizzes
            if(quiz == null)
            {
                return NotFound(new { Error = $"QuizId {id} has not been found" });
            }
            return new JsonResult(quiz.Adapt<QuizViewModel>(), base.JsonSettings);
        }

        /// <summary>
        /// Adds a new Quiz to the database
        /// </summary>
        /// <param name="model">The QuizViewModel containing the data to insert</param>
        [HttpPut]
        public IActionResult Put([FromBody] QuizViewModel model)
        {
            //return a generic HTTP Status 500(Server Error)
            //if the client payload is invalid
            if(model == null)
            {
                return new StatusCodeResult(500);
            }

            var quiz = new Quiz();

            //properties taken from the request
            quiz.Title = model.Title;
            quiz.Description = model.Description;
            quiz.Text = model.Text;
            quiz.Notes = model.Notes;

            //properties set from server-side
            quiz.CreatedDate = DateTime.Now;
            quiz.LastModifiedDate = quiz.CreatedDate;

            //set a temporary author using the Admin user's userId
            //as user login isn't supported yet -> we'll change this later on
            quiz.UserId = base.DbContext.Users.Where(user => string.Equals(user.UserName, "Admin"))
                .FirstOrDefault().Id;

            //add the new quiz to the database
            base.DbContext.Quizzes.Add(quiz);
            base.DbContext.SaveChanges();

            return new JsonResult(quiz.Adapt<QuizViewModel>(), JsonSettings);
        }

        /// <summary>
        /// Edit the Quiz with the given {id}
        /// </summary>
        /// <param name="model">The QuizViewModel containing the data to update</param>
        [HttpPost]
        public IActionResult Post([FromBody]QuizViewModel model)
        {
            //return a generic HTTP Status 500 (Server Error)
            //if the client payload is invalid
            if(model == null)
            {
                return new StatusCodeResult(500);
            }
            var quiz = base.DbContext.Quizzes.Where(q => q.Id == model.Id).FirstOrDefault();

            //handle requests asking for non-existing quizzes
            if(quiz == null)
            {
                return NotFound(new { Error = $"QuizId {model.Id} has not been found" });
            }

            //handle the update (without object-mapping)
            //by manually assigning the properties
            //we want to accept from the request

            quiz.Title = model.Title;
            quiz.Description = model.Description;
            quiz.Text = model.Text;
            quiz.Notes = model.Notes;

            //properties set from server-side
            quiz.LastModifiedDate = quiz.CreatedDate;

            //persist the changes into the database
            base.DbContext.SaveChanges();

            return new JsonResult(quiz.Adapt<QuizViewModel>(), JsonSettings);
        }

        /// <summary>
        /// Deletes the Quiz with the given {id} from the Database
        /// </summary>
        /// <param name="id">The Id of an existing Quiz</param>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            //retrieve the quiz from the database
            var quiz = base.DbContext.Quizzes.Where(q => q.Id == id).FirstOrDefault();

            //handle requests asking for non-existing quizzes
            if(quiz == null)
            {
                return NotFound(new { Error = $"QuizId {id} has not been found" });
            }

            //remove the quiz from the dbContext
            base.DbContext.Quizzes.Remove(quiz);
            base.DbContext.SaveChanges();

            //return an HTTP status 200 (OK)
            return new OkResult();
        }
        #endregion

        #region Attribute-based routing methods
        /// <summary>
        /// GET: api/quiz/latest
        /// Retrives the {num} latest quizzes
        /// </summary>
        /// <param name="num">the number of quizzes to retrieve</param>
        /// <returns>the {num} latest Quizzes</returns>
        [HttpGet("Latest/{num:int?}")]
        public IActionResult Latest(int num = 10)
        {
            var latest = base.DbContext.Quizzes
                .OrderByDescending(quiz => quiz.CreatedDate)
                .Take(num)
                .ToArray();
            return new JsonResult(latest.Adapt<QuizViewModel[]>(), base.JsonSettings);
        }

        /// <summary>
        /// GET: api/quiz/ByTitle
        /// Retrieves the {num} quizzes by Title (A to Z)
        /// </summary>
        /// <param name="num">The number of quizes to retrieve</param>
        /// <returns>{num} Quizzes sorterd by Title</returns>
        [HttpGet("ByTitle/{num:int?}")]
        public IActionResult ByTitle(int num = 10)
        {
            var byTitle = base.DbContext.Quizzes
                .OrderBy(quiz => quiz.Title)
                .Take(num)
                .ToArray();
            return new JsonResult(byTitle.Adapt<QuizViewModel[]>(), base.JsonSettings);
        }

        /// <summary>
        /// Get: api/quiz/Random
        /// Retrieves the {num} random quizzes
        /// </summary>
        /// <param name="num">the number of quizes to retrieve</param>
        /// <returns>{num} random quizzes</returns>
        [HttpGet("Random/{num:int?}")]
        public IActionResult Random(int num = 10)
        {
            var random = base.DbContext.Quizzes
                .OrderBy(quiz => Guid.NewGuid())
                .Take(num)
                .ToArray();
            return new JsonResult(random.Adapt<QuizViewModel[]>(), base.JsonSettings);
        }
        #endregion
    }
}
