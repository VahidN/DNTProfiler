using System;
using System.Globalization;
using System.Linq;
using DNTProfiler.TestEFContext.DataLayer;

namespace DNTProfiler.WebFormsTest
{
    public partial class MultipleContextPerRequest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (var ctx = new SampleContext())
            {
                lblCategories.Text = ctx.Categories.Count().ToString(CultureInfo.InvariantCulture);
            }

            using (var ctx = new SampleContext())
            {
                lblProducts.Text = ctx.Products.Count().ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}