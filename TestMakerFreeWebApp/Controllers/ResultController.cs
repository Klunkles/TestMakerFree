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
    public class ResultController : Controller
    {
        #region RESTful convention methods
        /// <summary>
        /// Retrieves the Result with the given {id}
        /// </summary>
        /// <param name="id">The Id of an existing Result</param>
        /// <returns>the Result with the given {id}</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Content("Not implemented (yet)!");
        }

        /// <summary>
        /// Adds a new Result to the database
        /// </summary>
        /// <param name="model">The ResultViewModel containing the data to insert</param>
        [HttpPut]
        public IActionResult Put(ResultViewModel model)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Edit the Result with the given {id}
        /// </summary>
        /// <param name="model">The ResultViewModel containing the data to update</param>
        [HttpPost]
        public IActionResult Post(ResultViewModel model)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes the Result with the given {id} from the Database
        /// </summary>
        /// <param name="id">The Id of an existing Result</param>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Attribute-based routing methods
        /// <summary>
        /// GET: api/Result/All
        /// </summary>
        /// <param name="quizId"></param>
        /// <returns></returns>
        [HttpGet("All/{quizId}")]
        public IActionResult All(int quizId)
        {
            var sampleResults = new List<ResultViewModel>();

            //add a first sample result
            sampleResults.Add(new ResultViewModel()
            {
                Id = 1,
                QuizId = quizId,
                Text = "What do you value most in your life?",
                CreatedDate = DateTime.Now,
                LastModifiedDate = DateTime.Now
            });

            for(int i = 2; i <=5; i++)
            {
                sampleResults.Add(new ResultViewModel()
                {
                    Id = i,
                    QuizId = quizId,
                    Text = $"Sample Question {i}",
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now
                });
            }

            return new JsonResult(sampleResults, new JsonSerializerSettings() { Formatting = Formatting.Indented });
        }
        #endregion
    }
}
