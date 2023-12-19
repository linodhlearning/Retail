using Microsoft.AspNetCore.Mvc;
using Retail.Messages;
using Retail.Model;
using Retail.Utils;

namespace Retail.Controllers
{
    [Route("retail")]
    [ApiController]
    public class ProductController : BaseController<ProductController>
    {
        public ProductController(ILogger<ProductController> logger) : base(logger)
        {
        }

        [HttpGet]
        [Route("products/{productId:int}")]
        public async Task<IActionResult> GetProduct(int productId)
        {
            try
            {
                return await SendToMessageHandler(new GetProductByIdRequest(productId));
            }
            catch (Exception ex)
            {
                string error = $"Error occurred while retrieving Product for Product Id:{productId}";
                _logger.LogError(ex, error);
                return ex.HandleException(error);
            }
        }

        [HttpGet]
        [Route("products")]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                return await SendToMessageHandler(new GetProductListRequest(null));
            }
            catch (Exception ex)
            {
                string error = $"Error occurred while retrieving Products";
                _logger.LogError(ex, error);
                return ex.HandleException(error);
            }
        }

        [HttpPost]
        [Route("product")]
        public async Task<IActionResult> AddProduct([FromBody] ProductModel model)
        {
            try
            {
                return await SendToMessageHandler(new AddProductRequest(model));
            }
            catch (Exception ex)
            {
                string error = $"Error occurred adding Product";
                _logger.LogError(ex, error);
                return ex.HandleException(error);
            }

        }

        [HttpPut]
        [Route("product")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductModel model)
        {
            try
            {
                return await SendToMessageHandler(new UpdateProductRequest(model));
            }
            catch (Exception ex)
            {
                string error = $"Error occurred while updating Product with Id {model?.Id}";
                _logger.LogError(ex, error);
                return ex.HandleException(error);
            }
        }


    }
}
