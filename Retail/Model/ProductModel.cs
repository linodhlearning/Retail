namespace Retail.Model
{
     
    public class ProductModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public Common.ProductTypes ProductType { get; set; }

        public decimal UnitPrice { get; set; }
    }
}
