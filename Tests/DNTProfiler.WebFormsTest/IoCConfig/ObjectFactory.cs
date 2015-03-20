using System;
using System.Threading;
using DNTProfiler.ServiceLayer;
using DNTProfiler.ServiceLayer.Contracts;
using DNTProfiler.TestEFContext.DataLayer;
using StructureMap;
using StructureMap.Web;

namespace DNTProfiler.WebFormsTest.IoCConfig
{
    public static class ObjectFactory
    {
        private static readonly Lazy<Container> _containerBuilder =
            new Lazy<Container>(defaultContainer, LazyThreadSafetyMode.ExecutionAndPublication);

        public static IContainer Container
        {
            get { return _containerBuilder.Value; }
        }

        private static Container defaultContainer()
        {
            return new Container(x =>
            {
                x.For<IUnitOfWork>().HybridHttpOrThreadLocalScoped().Use(() => new SampleContext());
                x.For<ICategoryService>().Use<EfCategoryService>();
                x.For<IProductService>().Use<EfProductService>();

                x.Policies.SetAllProperties(y =>
                {
                    y.OfType<IUnitOfWork>();
                    y.OfType<ICategoryService>();
                    y.OfType<IProductService>();
                });
            });
        }
    }
}