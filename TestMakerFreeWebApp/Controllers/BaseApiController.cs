using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestMakerFreeWebApp.Data;

namespace TestMakerFreeWebApp.Controllers
{
    [Route("api/[controller]")]
    public class BaseApiController : Controller
    {
        #region Shared Properties
        protected ApplicationDbContext DbContext { get; private set; }
        protected JsonSerializerSettings JsonSettings { get; private set; }
        #endregion

        #region Constructor
        public BaseApiController(ApplicationDbContext dbContext)
        {
            //Instantiate the ApplicationDbContext through Dependency Injection
            this.DbContext = dbContext;

            //Instatiate a single JsonSerializerSettings object that can be reused multiple times.
            this.JsonSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            };
        }
        #endregion
    }
}
