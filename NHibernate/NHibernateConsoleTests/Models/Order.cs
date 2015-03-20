using System;
using System.Collections.Generic;
using NHibernate.Validator.Constraints;

namespace NHibernateConsoleTests.Models
{
    public class Order
    {
        public Order()
        {
            Products = new List<Product>();
            Customer = new Customer();
        }

        public virtual int Id { set; get; }

        [NotNull]
        public virtual DateTime DateTime { set; get; }

        [NotNull]
        public virtual Customer Customer { set; get; } //Many-to-one Association

        public virtual IList<Product> Products { set; get; } //Many-to-many Association
    }
}
