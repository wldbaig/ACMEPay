using API.BusinessLogic;
using API.Common;
using API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using static API.Common.Enumeration;

namespace API.Web.Controllers
{
    /// <summary>
    /// Use this service to send AuthorizeService
    /// </summary> 
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthorizeTransactionController : APIBaseController<AuthorizeTransactionController>
    {
        public AuthorizeTransactionController(SettingConfig settingConfig, IServiceBusinessLogic serviceBusinessLogic, IServiceLogger<AuthorizeTransactionController> logger)
         : base(settingConfig, serviceBusinessLogic, logger)
        {
        }

        /// <summary>
        /// Use this api to get authorize transaction
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthorizationResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("/api/authorize/")]
        public async Task<IActionResult> AuthorizeRequest([FromBody] AuthorizationRequestDto dto)
        {
            MessageModel model = new();

            try
            {
                if (!ModelState.IsValid)
                {
                    throw new InvalidModelException();
                }
                var obj = await serviceBusinessLogic.AuthorizeTransaction(dto);
                model.Message = "Successfully save transaction";
                model.Type = MessageType.Success;
                model.HttpStatusCode = System.Net.HttpStatusCode.OK;
                return Ok(new { model, data = obj });
            }
            catch (InvalidModelException ex)
            {
                model.Message = ex.Message;
                model.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
                return ThrowInternalServerError(ex);
            }
            catch (Exception ex)
            {
                model.Message = ex.Message;
                model.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
                return ThrowInternalServerError(ex);
            }
        }


        /// <summary>
        /// Use this api to void transaction
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthorizationResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("/api/authorize/{id}/voids")]
        public async Task<IActionResult> VoidRequest(Guid id, [FromBody] UpdateTransactionRequestDto dto)
        {
            MessageModel model = new();

            try
            {
                if (!ModelState.IsValid)
                {
                    throw new InvalidModelException();
                }
                var obj = await serviceBusinessLogic.UpdateTransaction(id, dto, PaymentStatus.Voided);
                model.Message = "Successfully transaction to void";
                model.Type = MessageType.Success;
                model.HttpStatusCode = System.Net.HttpStatusCode.OK;
                return Ok(new { model, data = obj });
            }
            catch (InvalidModelException ex)
            {
                model.Message = ex.Message;
                model.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
                return ThrowInternalServerError(ex);
            }
            catch (Exception ex)
            {
                model.Message = ex.Message;
                model.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
                return ThrowInternalServerError(ex);
            }
        }


        /// <summary>
        /// Use this api to capture transaction
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthorizationResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("/api/authorize/{id}/capture")]
        public async Task<IActionResult> CaptureRequest(Guid id, [FromBody] UpdateTransactionRequestDto dto)
        {
            MessageModel model = new();

            try
            {
                if (!ModelState.IsValid)
                {
                    throw new InvalidModelException();
                }
                var obj = await serviceBusinessLogic.UpdateTransaction(id, dto, PaymentStatus.Captured);
                model.Message = "Successfully update transaction to Captured";
                model.Type = MessageType.Success;
                model.HttpStatusCode = System.Net.HttpStatusCode.OK;
                return Ok(new { model, data = obj });
            }
            catch (InvalidModelException ex)
            {
                model.Message = ex.Message;
                model.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
                return ThrowInternalServerError(ex);
            }
            catch (Exception ex)
            {
                model.Message = ex.Message;
                model.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
                return ThrowInternalServerError(ex);
            }
        }

       /// <summary>
       /// Use this to get all transactions
       /// </summary>
         
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(TransactionResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("/api/authorize/")]
        public async Task<IActionResult> GetAllTransactions([FromQuery] PaginationRequestDto paginationRequestDto)
        {
            MessageModel model = new();
            try
            {
                var obj = await serviceBusinessLogic.GetTransactions(paginationRequestDto);
                model.Message = "Successfully retrieve transactions";
                model.Type = MessageType.Success;
                model.HttpStatusCode = System.Net.HttpStatusCode.OK;
                return Ok(new { model, data = obj });
            }

            catch (Exception ex)
            {
                model.Message = ex.Message;
                model.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
                return ThrowInternalServerError(ex);
            }

        }
    }
}
