using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Retail.Model;

namespace Retail.Repository.Entity
{
    [Table("Product")]
    public class ProductEntity: EntityBase
    {
        public int Id { get; set; }
        [MaxLength(200)]
        public string Name { get; set; }
        public Common.ProductTypes ProductType { get; set; }

        public decimal UnitPrice { get; set; }
       
    }
}
