using System;
using DNTProfiler.ServiceLayer.Contracts;

namespace DNTProfiler.WebFormsTest
{
    public partial class IncorrectLazyLoading : BasePage
    {
        public IProductService ProductService { set; get; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GridView1.DataSource = ProductService.GetAllProducts();
                GridView1.DataBind();
            }
        }
    }
}