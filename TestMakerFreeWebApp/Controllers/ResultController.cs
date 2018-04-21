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
    public class ResultController : BaseApiController
    {
        #region Constructor
        public ResultController(ApplicationDbContext dbContext) : base(dbContext) { }
        #endregion

        #region RESTful convention methods
        /// <summary>
        /// Retrieves the Result with the given {id}
        /// </summary>
        /// <param name="id">The Id of an existing Result</param>
        /// <returns>the Result with the given {id}</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = base.DbContext.Results.Where(r => r.Id == id).FirstOrDefault();

            //handle requests asking for non-existing results
            if(result == null)
            {
                return NotFound(new { Error = $"ResultId {id} has not been found" });
            }

            return new JsonResult(result.Adapt<ResultViewModel>(), JsonSettings);
        }

        /// <summary>
        /// Adds a new Result to the database
        /// </summary>
        /// <param name="model">The ResultViewModel containing the data to insert</param>
        [HttpPut]
        public IActionResult Put([FromBody]ResultViewModel model)
        {
            //return a generic HTTP Status 500 (Server Error) if the client payload is invalid
            if(model == null)
            {
                return new StatusCodeResult(500);
            }

            //map the ViewModel to the Model
            var result = model.Adapt<Result>();

            //override those properties that should be set from the server-side only
            result.CreatedDate = DateTime.Now;
            result.LastModifiedDate = result.CreatedDate;

            //add the new result
            base.DbContext.Results.Add(result);
            base.DbContext.SaveChanges();

            //return the newly created Result to the client.
            return new JsonResult(result.Adapt<ResultViewModel>(), base.JsonSettings);
        }

        /// <summary>
        /// Edit the Result with the given {id}
        /// </summary>
        /// <param name="model">The ResultViewModel containing the data to update</param>
        [HttpPost]
        public IActionResult Post([FromBody]ResultViewModel model)
        {
            //return a generic HTTP Status 500 (server Error) if the client payload is invalid
            if(model == null)
            {
                return new StatusCodeResult(500);
            }

            //retrieve the result to edit
            var result = base.DbContext.Results.Where(r => r.Id == model.Id).FirstOrDefault();

            //handle requests asking for non-existing results
            if(result == null)
            {
                return NotFound(new { Error = $"ResultId {model.Id} has not been found" });
            }

            //handle the update (without object-mapping) by manually assigning the properties we want to accept from the request
            result.QuizId = model.QuizId;
            result.Text = model.Text;
            result.MinValue = model.MinValue;
            result.MaxValue = model.MaxValue;
            result.Notes = model.Notes;

            //properties set from server-side
            result.LastModifiedDate = result.CreatedDate;

            //persist the changes into the database
            base.DbContext.SaveChanges();

            //return the updated quiz to the client
            return new JsonResult(result.Adapt<ResultViewModel>(), base.JsonSettings);
        }

        /// <summary>
        /// Deletes the Result with the given {id} from the Database
        /// </summary>
        /// <param name="id">The Id of an existing Result</param>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            //retrieve the result from the database
            var result = base.DbContext.Results.Where(r => r.Id == id).FirstOrDefault();

            //handle the requests asking for non-existing results
            if(result == null)
            {
                return NotFound(new { Error = $"ResultId {id} has not been found" });
            }

            //remove the result from the dbContext
            DbContext.Results.Remove(result);
            DbContext.SaveChanges();

            //return an HTTP Status 200 (OK)
            return new OkResult();
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
            var results = base.DbContext.Results.Where(r => r.QuizId == quizId)
                .ToArray();

            return new JsonResult(results.Adapt<ResultViewModel[]>(), base.JsonSettings);
        }
        #endregion
    }
}
