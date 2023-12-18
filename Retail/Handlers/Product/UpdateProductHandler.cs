using AutoMapper;
using Retail.Messages;
using Retail.Model;
using Retail.Repository;
using Retail.Repository.Entity;
using Retail.Utils;

namespace Retail.Handlers.Product
{
    public class  UpdateProductHandler : BaseMessageHandler< UpdateProductRequest,  UpdateProductResponse>
    {
        private readonly ILogger< UpdateProductHandler> _logger;
        private readonly RepoUow _repo;
        private readonly IMapper _mapper;
        private ProductEntity _product;

        public  UpdateProductHandler(ILogger< UpdateProductHandler> logger, RepoUow repo, IMapper mapper) : base(logger)
        {
            _logger = logger;
            _repo = repo;
            _mapper = mapper;
        }

        public override async Task<RetailInProcessResponse> ValidateMessage( UpdateProductRequest message)
        {
            var errors = new List<ErrorModel>();
            var id = message.Model.Id;
            _product = _repo.Product.GetById(id);
            if (_product == null)
            {
                errors.Add(new ErrorModel
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Title = $"Product id {id} not found.",
                    Detail = $"Product with id {id} not found in the system to update.",
                    Source = string.Empty
                });
            }
            return await Task.FromResult(new RetailInProcessResponse { Errors = errors });
        }

        public override async Task< UpdateProductResponse> HandleMessage( UpdateProductRequest request)
        {
            _logger.LogInformation(" UpdateProductHandler HandleMessage Begin");
             _product=_mapper.Map<ProductEntity>(request.Model);
             _product.ModifiedOn = DateTimeOffset.Now;
             _product.ModifiedBy = HttpContextData.GetUser();
            _repo.Product.Update(_product);
            _repo.Save();
             var data = _mapper.Map<ProductModel>(_product);
            _logger.LogInformation("UpdateProductHandler HandleMessage End");
            return await Task.FromResult(new  UpdateProductResponse(data));
        }

    }
}
