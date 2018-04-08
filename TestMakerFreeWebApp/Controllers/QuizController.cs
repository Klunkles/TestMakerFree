using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TestMakerFreeWebApp.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestMakerFreeWebApp.Controllers
{
    [Route("api/[controller]")]
    public class QuizController : Controller
    {
        private JsonSerializerSettings jsonSerializer = new JsonSerializerSettings() { Formatting = Formatting.Indented };

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
            //Create sample quiz to match the given request
            var quiz = new QuizViewModel()
            {
                Id = id,
                Title = $"Sample quiz with Id = {id}",
                Description = "Not a real quiz; it's just a sample",
                CreatedDate = DateTime.Now,
                LastModifiedDate = DateTime.Now
            };


            return new JsonResult(quiz, jsonSerializer);
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
            var sampleQuizzes = new List<QuizViewModel>();
            sampleQuizzes.Add(new QuizViewModel()
            {
                Id = 1,
                Title = "Which Shingki No Kyojin character are you?",
                Description = "Anime-related personality test",
                CreatedDate = DateTime.Now,
                LastModifiedDate = DateTime.Now
            });

            for(int i = 2; i <= num; i++)
            {
                sampleQuizzes.Add(new QuizViewModel()
                {
                    Id = i,
                    Title = $"Sample Quiz {i}",
                    Description = "This is a sample quiz",
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now
                });
            }

            return new JsonResult(sampleQuizzes, jsonSerializer);
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
            var sampleQuizzes = ((JsonResult)Latest(num)).Value as List<QuizViewModel>;
            return new JsonResult(sampleQuizzes.OrderBy(quiz => quiz.Title), jsonSerializer);
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
            var sampleQuizzes = ((JsonResult)Latest(num)).Value as List<QuizViewModel>;
            return new JsonResult(sampleQuizzes.OrderBy(quiz => Guid.NewGuid()), jsonSerializer);
        }
        #endregion
    }
}
