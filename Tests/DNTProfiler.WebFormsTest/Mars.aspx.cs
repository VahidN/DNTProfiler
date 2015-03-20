using System;
using DNTProfiler.ServiceLayer.Contracts;

namespace DNTProfiler.WebFormsTest
{
    public partial class Mars : BasePage
    {
        public ICategoryService Service { set; get; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GridView1.DataSource = Service.IssueIncorrectLazyLoading();
                GridView1.DataBind();
            }
        }
    }
}