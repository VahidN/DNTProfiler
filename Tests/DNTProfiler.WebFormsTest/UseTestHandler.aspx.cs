using System;
using System.Globalization;
using DNTProfiler.ServiceLayer.Contracts;

namespace DNTProfiler.WebFormsTest
{
    public partial class UseTestHandler : BasePage
    {
        public IProductService ProductService { set; get; }

        protected void Page_Load(object sender, EventArgs e)
        {
            var sum = ProductService.GetTotalPriceSumLong();
            lblSum.Text = sum.ToString(CultureInfo.InvariantCulture);
        }
    }
}