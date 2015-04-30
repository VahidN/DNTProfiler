using System.Collections.Generic;
using DNTProfiler.TestEFContext.Domain;

namespace DNTProfiler.ServiceLayer.Contracts
{
    public interface IProductService
    {
        void AddNewProduct(Product product);
        IList<Product> GetAllProductsIncludeCategory();
        IList<Product> GetAllProductsStartWith(string name);
        double GetTotalPriceSumInt();
        long GetTotalPriceSumLong();
        IList<Product> GetAllProducts();
        IEnumerable<Product> GetAllProductsWithPriceGreaterThan(int value);
        Product FindProduct(string name);
        IList<Product> GetProductsWithMultipleJoins();
        IList<Product> GetAllProductsAsCacheableList();
        IList<Product> GetProductsWithMultipleJoinsWithLet();
    }
}