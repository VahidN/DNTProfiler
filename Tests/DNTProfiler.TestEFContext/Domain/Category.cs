using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DNTProfiler.TestEFContext.Domain
{
    public class Category
    {
        public int Id { get; set; }

        [Index(IsUnique = true)]
        [MaxLength(450)]
        [Required]
        public string Name { get; set; }

        [MaxLength(450)]
        public string Title { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public int? UserId { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}