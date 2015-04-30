using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DNTProfiler.TestEFContext.DataLayer;
using DNTProfiler.TestEFContext.Domain;
using System;
using DNTProfiler.ServiceLayer.Contracts;
using EFSecondLevelCache;

namespace DNTProfiler.ServiceLayer
{
    public class EfProductService : IProductService
    {
        IUnitOfWork _uow;
        readonly IDbSet<Product> _products;
        public EfProductService(IUnitOfWork uow)
        {
            _uow = uow;
            _products = _uow.Set<Product>();
        }

        public void AddNewProduct(Product product)
        {
            _products.Add(product);
        }

        public IList<Product> GetAllProductsIncludeCategory()
        {
            return _products.Include(x => x.Category).ToList();
        }

        public IList<Product> GetAllProducts()
        {
            return _products.ToList();
        }

        public IEnumerable<Product> GetAllProductsWithPriceGreaterThan(int value)
        {
            return _products.Where(x => x.Price > value);
        }

        public IList<Product> GetAllProductsStartWith(string name)
        {
            return _products.Where(x => x.Name.StartsWith(name)).ToList();
        }

        public double GetTotalPriceSumInt()
        {
            return _products.Sum(x => x.Price);
        }

        public long GetTotalPriceSumLong()
        {
            return _products.Sum(x => (Int64)x.Price);
        }

        public Product FindProduct(string name)
        {
            return _products.FirstOrDefault(x => x.Name.ToUpper() == name.ToUpper());
        }

        public IList<Product> GetProductsWithMultipleJoins()
        {
            return _products.Where(product => product.Id > 1)
                            .Include(product => product.Category)
                            .Include(product => product.User)
                            .Where(product => product.Category.Title.Contains("c") && product.Category.Id > 1 && product.Price > 100)
                            .OrderBy(product => product.Price)
                            .ToList();
        }

        public IList<Product> GetProductsWithMultipleJoinsWithLet()
        {
            var query = from product in _products
                        let category = product.Category
                        where product.Id > 1
                        where category.Title.Contains("c") && category.Id > 1 && product.Price > 100
                        select new { product, category };

            // its equivalent form, `Let` in chained extension methods
            var query2 = _products.Include(product => product.Category)
                .Include(product => product.User)
                .Select(product => new { product, category = product.Category }) // to avoid duplicate joins to the same table
                .Where(@t => @t.product.Id > 1)
                .Where(@t => @t.category.Title.Contains("c") && @t.category.Id > 1 && @t.product.Price > 100)
                .Select(@t => new {@t.product, @t.category});

            return query.ToList().Select(x => x.product).ToList();
        }

        public IList<Product> GetAllProductsAsCacheableList()
        {
            return _products.Cacheable().ToList();
        }
    }
}