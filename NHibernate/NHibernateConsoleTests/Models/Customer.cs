using System.Collections.Generic;
using NHibernate.Validator.Constraints;

namespace NHibernateConsoleTests.Models
{
    public class Customer
    {
        public Customer()
        {
            Orders = new List<Order>();
            Address = new Address();
        }

        public virtual int Id { set; get; }

        [Length(Max = 450)]
        [NotNullNotEmpty]
        public virtual string Name { set; get; }

        [NotNull]
        public virtual Address Address { set; get; } //Many-to-one Association

        public virtual IList<Order> Orders { set; get; } //One-to-many Association
    }
}
