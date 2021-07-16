using API.BusinessLogic;
using API.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace API.Web.Controllers
{
    public abstract class APIBaseController<T> : ControllerBase where T : class
    {
        protected readonly IServiceBusinessLogic serviceBusinessLogic;
        protected readonly SettingConfig settingConfig; 
        protected readonly IServiceLogger<T> logger;

        protected APIBaseController(SettingConfig settingConfig, IServiceBusinessLogic serviceBusinessLogic, IServiceLogger<T> logger)
        {
            this.serviceBusinessLogic = serviceBusinessLogic;
            this.settingConfig = settingConfig;
            this.logger = logger;  
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult ThrowInternalServerError(Exception ex, string message = "")
        {
            logger.Error(ex.Message, ex);
            
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                Message = "An error has occurred.",
                ExceptionMessage = message,
                ExceptionType = "Exception"
            });
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult ThrowBadRequestError(string errorMessage)
        {
            return StatusCode(StatusCodes.Status400BadRequest, new
            {
                Message = errorMessage
            });
        }
    }
}