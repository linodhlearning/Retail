using Retail.Model;

namespace Retail.Messages
{

    public record GetProductByIdRequest(int Id) : BaseInProcessMessage;
    public record GetProductByIdResponse(ProductModel? Data) : RetailInProcessResponse;


    public record GetProductListRequest(Common.ProductTypes? ProductType) : BaseInProcessMessage;
    public record GetProductListResponse(IEnumerable<ProductModel> Data) : RetailInProcessResponse;


    public record AddProductRequest(ProductModel Model) : BaseInProcessMessage;
    public record AddProductResponse(int Id) : RetailInProcessResponse;


    public record UpdateProductRequest(ProductModel Model) : BaseInProcessMessage;
    public record UpdateProductResponse(ProductModel Model) : RetailInProcessResponse;


}
