using LimetimePhotoUploadUI.Models.ZelbikeChrono;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LimetimePhotoUploadUI.Controllers
{
    public abstract class GenericController : Controller
    {
        protected ZelbikeChronoContext Entities { get; private set; }
        protected IConfiguration Configuration;



        protected GenericController(IConfiguration configuration, ZelbikeChronoContext entities)
        {
            this.Configuration = configuration;
            this.Entities = entities;
        }

        protected ActionResult FailResult(string message = null, object data = null)
        {
            return this.Content(JsonConvert.SerializeObject(
                new
                {
                    isSuccess = false,
                    message,
                    data
                },
                new JsonSerializerSettings()
                {
                    Formatting = Newtonsoft.Json.Formatting.Indented,
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                }), "application/json");
        }

        protected ActionResult SuccessResult(object data = null)
        {
            return this.Content(JsonConvert.SerializeObject(
                new
                {
                    isSuccess = true,
                    data
                },
                new JsonSerializerSettings()
                {
                    Formatting = Newtonsoft.Json.Formatting.Indented,
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                }), "application/json");
        }
    }
}
