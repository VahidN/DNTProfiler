using System;
using DNTProfiler.ServiceLayer.Contracts;
using DNTProfiler.TestEFContext.DataLayer;
using DNTProfiler.TestEFContext.Domain;

namespace DNTProfiler.WebFormsTest
{
    public partial class WebForm3 : BasePage
    {
        public IUnitOfWork UoW { set; get; }
        public IProductService ProductService { set; get; }
        public ICategoryService CategoryService { set; get; }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            var category = new Category
            {
                Name = txtName.Text,
                Title = txtTitle.Text
            };
            CategoryService.AddNewCategory(category);
            UoW.SaveAllChanges();
            Response.Redirect("~/Default.aspx");
        }
    }
}