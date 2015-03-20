using System.Collections.Generic;

namespace DNTProfiler.TestEFContext.Domain
{
    public class User
    {
        public int Id { set; get; }
        public string Name { set; get; }

        public virtual ICollection<Category> Categories { set; get; }
        public virtual ICollection<Product> Products { set; get; }
    }
}