using AutoMapper;
using Retail.Messages;
using Retail.Model;
using Retail.Repository;

namespace Retail.Handlers.Product
{
    public class GetProductListHandler : BaseMessageHandler<GetProductListRequest, GetProductListResponse>
    {
        private readonly ILogger<GetProductListHandler> _logger;
        private readonly RepoUow _repo;
        private readonly IMapper _mapper;

        public GetProductListHandler(ILogger<GetProductListHandler> logger, RepoUow repo, IMapper mapper) : base(logger)
        {
            _logger = logger;
            _repo = repo;
            _mapper = mapper;
        }

        public override async Task<RetailInProcessResponse> ValidateMessage(GetProductListRequest message)
        { 
            return await Task.FromResult(new RetailInProcessResponse { Errors = new List<ErrorModel>() });
        }

        public override async Task<GetProductListResponse> HandleMessage(GetProductListRequest message)
        {
            _logger.LogInformation("GetProductListHandler HandleMessage Begin");
            var products = _repo.Product.GetAll();
            var data = _mapper.Map<List<ProductModel>>(products);
            _logger.LogInformation("GetProductListHandler HandleMessage End");
            return await Task.FromResult(new GetProductListResponse(data));
        }


    }
}
