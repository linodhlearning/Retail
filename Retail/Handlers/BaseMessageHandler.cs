using MediatR;
using Retail.Messages;
using Retail.Model;
using System.Diagnostics;
using System.Text;

namespace Retail.Handlers
{
    public abstract class BaseMessageHandler<TMessage, TResponse> :
         INotificationHandler<TMessage>,
         IRequestHandler<TMessage, RetailInProcessResponse>
         where TMessage : IRequest<RetailInProcessResponse>, INotification
         where TResponse : RetailInProcessResponse
    {
        protected ILogger _logger;
        protected RetailInProcessResponse ValidationResult;

        public BaseMessageHandler(ILogger logger)
        {
            this._logger = logger;
        }

        Task<RetailInProcessResponse> IRequestHandler<TMessage, RetailInProcessResponse>.Handle(TMessage message, CancellationToken cancellationToken)
        {
            return this.HandleInternal(message, cancellationToken);
        }

        Task INotificationHandler<TMessage>.Handle(TMessage notification, CancellationToken cancellationToken)
        {
            return (Task)this.HandleInternal(notification, cancellationToken);
        }

        private async Task<RetailInProcessResponse> HandleInternal(
          TMessage message,
          CancellationToken cancellationToken)
        {
            try
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                try
                {
                    this.ValidationResult = await this.ValidateMessage(message);
                    return this.ValidationResult.Errors != null && this.ValidationResult.Errors.Any<ErrorModel>() ? this.ValidationResult : (RetailInProcessResponse)await this.HandleMessage(message);
                }
                finally
                {
                    stopWatch.Stop();
                    var strHandler = new StringBuilder();
                    strHandler.Append(nameof(message));
                    strHandler.Append(": action executed in ");
                    strHandler.Append(stopWatch.ElapsedMilliseconds);
                    strHandler.Append(" milliseconds");
                    string stringAndClear = strHandler.ToString();
                    object[] objArray = Array.Empty<object>();
                    this._logger.LogInformation(stringAndClear, objArray);
                }
            }
            catch (Exception ex)
            {
                Exception exception = (Exception)(ex as AggregateException).Flatten() ?? ex;
                var strHandler = new StringBuilder(56, 3);
                strHandler.Append("Request(message) Type: ");
                strHandler.Append(typeof(TMessage).Name);
                strHandler.Append(", Response Type: ");
                strHandler.Append(typeof(TResponse).Name);
                strHandler.Append(", MessageObject:");
                strHandler.Append(message);
                string stringAndClear = strHandler.ToString();
                this._logger.LogError(exception, stringAndClear);
                return new RetailInProcessResponse()
                {
                    Errors = new List<ErrorModel>
                    {
                        new ErrorModel
                        {
                            Source = "BaseHandler",
                            Title = "Internal exception occured. Refer to logs for details.",
                            Detail = stringAndClear
                        }
                    }
                };
            }
        }

        public abstract Task<RetailInProcessResponse> ValidateMessage(TMessage message);

        public abstract Task<TResponse> HandleMessage(TMessage message);
    }
}
