using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DNTProfiler.TestEFContext.Domain
{
    [Table("MyProducts")]
    public class Product
    {
        [Key]
        [Column(name: "MyProductId")]
        public int Id { get; set; }

        [Index(IsUnique = true)]
        [MaxLength(450)]
        [Required]
        public string Name { get; set; }

        public double Price { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        public int? CategoryId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public int? UserId { get; set; }
    }
}