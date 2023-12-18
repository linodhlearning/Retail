using System.ComponentModel.DataAnnotations;

namespace Retail.Repository.Entity
{
    public interface IEntity
    {
        public int Id { get; set; } 
    }

    public class EntityBase : IEntity
    {
        [Required]
        public int Id { get; set; }

        [MaxLength(200)]
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedOn { get; set; }

        [MaxLength(200)]
        public string? ModifiedBy { get; set; }
        public DateTimeOffset? ModifiedOn { get; set; }
    }
}
