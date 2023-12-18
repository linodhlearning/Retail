using AutoMapper;
using Retail.Messages;
using Retail.Model;
using Retail.Repository;
using Retail.Repository.Entity;
using Retail.Utils;

namespace Retail.Handlers.Product
{
    public class AddProductHandler : BaseMessageHandler<AddProductRequest, AddProductResponse>
    {
        private readonly ILogger<AddProductHandler> _logger;
        private readonly RepoUow _repo;
        private readonly IMapper _mapper;


        public AddProductHandler(ILogger<AddProductHandler> logger, RepoUow repo, IMapper mapper) : base(logger)
        {
            _logger = logger;
            _repo = repo;
            _mapper = mapper;
        }

        public override async Task<RetailInProcessResponse> ValidateMessage(AddProductRequest message)
        {
            var errors = new List<ErrorModel>();
            return await Task.FromResult(new RetailInProcessResponse { Errors = errors });
        }

        public override async Task<AddProductResponse> HandleMessage(AddProductRequest request)
        {
            _logger.LogInformation("AddProductHandler HandleMessage Begin");

            var entity = _mapper.Map<ProductEntity>(request.Model);
            entity.Id = 0;
            entity.CreatedOn = DateTimeOffset.Now;
            entity.CreatedBy = HttpContextData.GetUser();
            _repo.Product.Create(entity);
            _repo.Save();
            //var data = _mapper.Map<ProductModel>(entity);
            _logger.LogInformation("AddProductHandler HandleMessage End");
            return await Task.FromResult(new AddProductResponse(entity.Id));
        }

    }
}
