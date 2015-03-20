using System.Collections.Generic;
using DNTProfiler.TestEFContext.Domain;

namespace DNTProfiler.ServiceLayer.Contracts
{
    public interface ICategoryService
    {
        void AddNewCategory(Category category);
        IList<Category> GetAllCategories();
        Category FindCategory(int id);
        IList<Category> GetNullCategoryTitles();
        IList<Category> GetNullCategoryTitlesSolution1();
        IList<Category> GetNullCategoryNames();
        IList<Category> GetNullCategoryNamesSolution1();
        IList<string> IssueIncorrectLazyLoading();
        int GetFirstCategoryProductsCount();
    }
}