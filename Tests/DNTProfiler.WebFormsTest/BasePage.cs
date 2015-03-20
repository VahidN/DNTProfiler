using System.Web.UI;

namespace DNTProfiler.WebFormsTest
{
    public class BasePage : Page
    {
        public BasePage()
        {
            IoCConfig.ObjectFactory.Container.BuildUp(this);
        }
    }
}