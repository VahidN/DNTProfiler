using System;
using NHibernateConsoleTests.Core;
using NHibernateConsoleTests.Models;

namespace NHibernateConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new NHConfig
            {
                ConnectionString = "Data Source=(local);Initial Catalog=testdb1;Integrated Security = true",
                MappingsAssembly = typeof(Product).Assembly,
                MappingsNamespace = typeof(Product).Namespace,
                ValidationDefinitionsNamespace = typeof(Product).Namespace,
                ShowLogs = true,
                DropTablesCreateDbSchema = true,
                OutputXmlMappingsFile = "Mappings.xml",
                DbSchemaOutputFile = "Db.sql",
                AutoMappingOverride = modelMapper =>
                {
                    modelMapper.BeforeMapProperty += (modelInspector, member, map) =>
                    {
                        if (member.LocalMember.Name.Equals("Name"))
                        {
                            map.Unique(true);
                        }
                    };
                }
            };

            var addresses = InMemoryDataSource.CreateAddresses();
            var customers = InMemoryDataSource.CreateCustomers(addresses);
            var products = InMemoryDataSource.CreateProducts();
            var orders = InMemoryDataSource.CreateOrders(customers, products);

            using (var sessionFactory = config.SetUpSessionFactory())
            {
                using (var session = sessionFactory.OpenSession())
                {
                    using (var tx = session.BeginTransaction())
                    {
                        // Save Products
                        foreach (var product in products)
                            session.SaveOrUpdate(product);

                        // Save Orders (also saves Customers and their Addresses)
                        foreach (var order in orders)
                            session.SaveOrUpdate(order);

                        tx.Commit();
                    }
                }

                using (var session = sessionFactory.OpenSession())
                {
                    var productsList = session.QueryOver<Product>().List();
                    foreach (var item in productsList)
                    {
                        Console.WriteLine("Name: " + item.Name);
                    }
                }

                using (var session = sessionFactory.OpenSession())
                {
                    var productsList = session.CreateSQLQuery("select * from Products where name like 'Widget%'")
                                              .AddEntity(typeof(Product))
                                              .List<Product>();
                    foreach (var item in productsList)
                    {
                        Console.WriteLine("Name: " + item.Name);
                    }
                }
            }


            Console.WriteLine("Press a key...");
            Console.ReadKey();
        }
    }
}