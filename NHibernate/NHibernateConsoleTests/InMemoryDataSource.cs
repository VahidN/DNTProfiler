using System;
using System.Collections.Generic;
using NHibernateConsoleTests.Models;

namespace NHibernateConsoleTests
{
    public class InMemoryDataSource
    {
        public static IList<Address> CreateAddresses()
        {
            return new List<Address>
            {
                      new Address
                     {
                        StreetAddress = "100 Main Street",
                        City = "Chicago",
                        StateName = "IL",
                        Zip = "60601"
                     },
                     new Address
                     {
                        StreetAddress = "200 Main Street",
                        City = "Chicago",
                        StateName = "IL",
                        Zip = "60601"
                     },
                     new Address
                     {
                        StreetAddress = "100 Main Street",
                        City = "Chicago",
                        StateName = "IL",
                        Zip = "60601"
                     }
            };
        }

        public static IList<Customer> CreateCustomers(IList<Address> addresses)
        {
            return new List<Customer>
            {
                new Customer
                {
                     Name = "Able, Inc.",
                     Address = addresses[0]
                },
                new Customer
                {
                     Name = "Baker, Inc.",
                     Address = addresses[1]
                },
                new Customer
                {
                     Name = "Charlie, Inc.",
                     Address = addresses[2]
                }
            };
        }

        public static IList<Order> CreateOrders(IList<Customer> customers, IList<Product> products)
        {
            var results = new List<Order>();
            var random = new Random(DateTime.Now.Millisecond);

            foreach (var customer in customers)
            {
                var order = new Order
                {
                    Products = new List<Product>
                    {
                        products[random.Next(2,5)],
                        products[random.Next(1,9)]
                    },
                    DateTime = DateTime.Now,
                    Customer = customer
                };

                foreach (var product in order.Products)
                {
                    product.Orders.Add(order);
                }
                customer.Orders.Add(order);

                results.Add(order);
            }
            return results;
        }

        public static IList<Product> CreateProducts()
        {
            var results = new List<Product>();
            for (int i = 0; i < 10; i++)
            {
                var newProduct = new Product
                {
                    Name = string.Format("Widget, Type {0:D2}", i)
                };
                results.Add(newProduct);
            }
            return results;
        }
    }
}