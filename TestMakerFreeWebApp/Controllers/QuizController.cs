using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TestMakerFreeWebApp.Data;
using TestMakerFreeWebApp.ViewModels;
using Mapster;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestMakerFreeWebApp.Controllers
{
    [Route("api/[controller]")]
    public class QuizController : Controller
    {
        #region Private Fields
        private ApplicationDbContext dbContext;

        private JsonSerializerSettings jsonSerializer = new JsonSerializerSettings() { Formatting = Formatting.Indented };

        #endregion

        #region Constructor
        public QuizController(ApplicationDbContext dbContext)
        {
            //Instatiate the ApplicationDbContext through Dependency Injection
            this.dbContext = dbContext;
        }
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
            var quiz = dbContext.Quizzes.Where(q => q.Id == id).FirstOrDefault();
            return new JsonResult(quiz.Adapt<QuizViewModel>(), jsonSerializer);
        }

        /// <summary>
        /// Adds a new Quiz to the database
        /// </summary>
        /// <param name="model">The QuizViewModel containing the data to insert</param>
        [HttpPut]
        public IActionResult Put(QuizViewModel model)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Edit the Quiz with the given {id}
        /// </summary>
        /// <param name="model">The QuizViewModel containing the data to update</param>
        [HttpPost]
        public IActionResult Post(QuizViewModel model)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes the Quiz with the given {id} from the Database
        /// </summary>
        /// <param name="id">The Id of an existing Quiz</param>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            throw new NotImplementedException();
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
            var latest = dbContext.Quizzes
                .OrderByDescending(quiz => quiz.CreatedDate)
                .Take(num)
                .ToArray();
            return new JsonResult(latest.Adapt<QuizViewModel[]>(), jsonSerializer);
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
            var byTitle = dbContext.Quizzes
                .OrderBy(quiz => quiz.Title)
                .Take(num)
                .ToArray();
            return new JsonResult(byTitle.Adapt<QuizViewModel[]>(), jsonSerializer);
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
            var random = dbContext.Quizzes
                .OrderBy(quiz => Guid.NewGuid())
                .Take(num)
                .ToArray();
            return new JsonResult(random.Adapt<QuizViewModel[]>(), jsonSerializer);
        }
        #endregion
    }
}
