using MediatR;
using Retail.Model;

namespace Retail.Messages
{
    public record BaseInProcessMessage() : IRequest<RetailInProcessResponse>,  INotification; 

    public record RetailInProcessResponse()
    {
        public List<ErrorModel> Errors { get; set; }

        public List<ErrorModel> Warnings { get; set; }

        public List<ErrorModel> Infos { get; set; }
    }
}
