using NHibernate.Validator.Constraints;

namespace NHibernateConsoleTests.Models
{
    public class Address
    {
        public virtual int Id { set; get; }

        [Length(Max = 450)]
        [NotNullNotEmpty]
        public virtual string City { set; get; }

        [Length(Max = 650)]
        [NotNullNotEmpty]
        public virtual string StreetAddress { set; get; }

        [Length(Max = 150)]
        [NotNullNotEmpty]
        public virtual string StateName { set; get; }

        [Length(Max = 50)]
        public virtual string Zip { set; get; }
    }
}
