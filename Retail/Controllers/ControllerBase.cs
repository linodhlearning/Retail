using MediatR;
using Microsoft.AspNetCore.Mvc;
using Retail.Messages;
using Retail.Model;

namespace Retail.Controllers
{
    public class BaseController<T> : ControllerBase where T : ControllerBase
    {
        protected ILogger<T> _logger;
        public BaseController(ILogger<T> logger)
        {
            _logger = logger;
        }

        internal string GetTransactionId()
        {
            const string TransactionIdHeaderName = "X-transactionId";
            string transactionId = this.HttpContext?.Request.Headers[TransactionIdHeaderName];
            // Generate the transaction id if one does not already exist
            return string.IsNullOrWhiteSpace(transactionId)
                ? Guid.NewGuid().ToString().Trim('{', '}')
                : transactionId;
        }

        [NonAction]
        public async Task<IActionResult> SendToMessageHandler(object request, CancellationToken cancellationToken = default(CancellationToken))
        {
            BaseController<T> ctrlBase = this;
            object obj = await this.HttpContext.RequestServices.GetService<IMediator>().Send(request, cancellationToken);
            if (obj != null && (object)(obj as RetailInProcessResponse) != null)
            {
                var inProcessResponse = obj as RetailInProcessResponse;
                if (inProcessResponse != null && (inProcessResponse.Errors == null || !inProcessResponse.Errors.Any<ErrorModel>()))
                {
                    return (IActionResult)ctrlBase.Ok((object)inProcessResponse);
                }
                else if (inProcessResponse.Errors.Any(c => c.Code == StatusCodes.Status404NotFound))
                {
                    return (IActionResult)ctrlBase.NotFound((object)inProcessResponse);
                }
                else
                {
                    return (IActionResult)ctrlBase.UnprocessableEntity((object)inProcessResponse);
                }
                //return (inProcessResponse.Errors == null || !inProcessResponse.Errors.Any<ErrorModel>()) ?
                //        (IActionResult)ctrlBase.Ok((object)inProcessResponse) : (IActionResult)ctrlBase.NotFound((object)inProcessResponse);
            }
            return ctrlBase.Problem("Response is either not of type 'RetailInProcessResponse' or is null", ctrlBase.GetType().Name);
        }

    }
}
