using AutoMapper;
using Retail.Messages;
using Retail.Model;
using Retail.Repository;
using Retail.Repository.Entity;

namespace Retail.Handlers.Product
{
    public class GetProductByIdHandler : BaseMessageHandler<GetProductByIdRequest, GetProductByIdResponse>
    {
        private readonly ILogger<GetProductByIdHandler> _logger;
        private readonly RepoUow _repo;
        private readonly IMapper _mapper;
        private ProductEntity _product;
        public GetProductByIdHandler(ILogger<GetProductByIdHandler> logger, RepoUow repo, IMapper mapper) : base(logger)
        {
            _logger = logger;
            _repo = repo;
            _mapper = mapper;
        }

        public override async Task<RetailInProcessResponse> ValidateMessage(GetProductByIdRequest message)
        {
            _logger.LogInformation("GetProductByIdHandler ValidateMessage Begin");
            var errors = new List<ErrorModel>();
            _product = _repo.Product.GetById(message.Id);
            if (_product == null)
            {
                errors.Add(new ErrorModel
                {
                    Code = StatusCodes.Status404NotFound,
                    Title = $"Product id {message.Id} not found.",
                    Detail = $"Requested Product with id {message.Id} not found in the system.",
                    Source = string.Empty
                });
            }
            _logger.LogInformation("GetProductByIdHandler ValidateMessage End");
            return await Task.FromResult(new RetailInProcessResponse { Errors = errors });
        }

        public override async Task<GetProductByIdResponse> HandleMessage(GetProductByIdRequest message)
        {
            _logger.LogInformation("GetProductByIdHandler HandleMessage Begin");
            var data = _mapper.Map<ProductModel>(_product);
            _logger.LogInformation("GetProductListHandler HandleMessage End");
            return await Task.FromResult(new GetProductByIdResponse(data));
        }


    }
}
