using System.Collections.Generic;
using NHibernate.Validator.Constraints;

namespace NHibernateConsoleTests.Models
{
    public class Product
    {
        public Product()
        {
            Orders = new List<Order>();
        }

        public virtual int Id { set; get; }

        [Length(Max = 450)]
        [NotNullNotEmpty]
        public virtual string Name { set; get; }

        public virtual IList<Order> Orders { set; get; } //Many-to-many Association
    }
}
