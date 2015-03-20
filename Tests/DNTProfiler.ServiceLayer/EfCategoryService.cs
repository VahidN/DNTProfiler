using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DNTProfiler.ServiceLayer.Contracts;
using DNTProfiler.TestEFContext.DataLayer;
using DNTProfiler.TestEFContext.Domain;

namespace DNTProfiler.ServiceLayer
{
    public class EfCategoryService : ICategoryService
    {
        IUnitOfWork _uow;
        readonly IDbSet<Category> _categories;
        public EfCategoryService(IUnitOfWork uow)
        {
            _uow = uow;
            _categories = _uow.Set<Category>();
        }

        public void AddNewCategory(Category category)
        {
            _categories.Add(category);
        }

        public IList<Category> GetAllCategories()
        {
            return _categories.ToList();
        }

        public Category FindCategory(int id)
        {
            return _categories.Find(id);
        }

        public IList<Category> GetNullCategoryTitles()
        {
            string name = null; // It won't produce `is null` in the final SQL.
            return _categories.Where(x => x.Title == name).ToList();
        }

        public IList<Category> GetNullCategoryTitlesSolution1()
        {
            return _categories.Where(x => x.Title == null).ToList();
        }

        public IList<Category> GetNullCategoryNames()
        {
            string name = null; // It won't produce `is null` in the final SQL.
            return _categories.Where(x => x.Name == name).ToList();
        }

        public IList<Category> GetNullCategoryNamesSolution1()
        {
            return _categories.Where(x => x.Name == null).ToList();
        }

        public IList<string> IssueIncorrectLazyLoading()
        {
            var list = new List<string>();
            foreach (var category in _categories.AsEnumerable())
            {
                foreach (var product in category.Products)
                {
                    if (product == null) continue;
                    list.Add(string.Format("{0}:{1}", product.Id, product.Name));
                }
            }
            return list;
        }

        public int GetFirstCategoryProductsCount()
        {
            return _categories.First().Products.Count;
        }
    }
}