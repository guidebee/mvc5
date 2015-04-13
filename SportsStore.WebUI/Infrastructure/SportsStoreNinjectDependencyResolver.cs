using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Concrete;

namespace SportsStore.WebUI.Infrastructure
{
    public class SportsStoreNinjectDependencyResolver:IDependencyResolver
    {
        private readonly IKernel _kernel;

        public SportsStoreNinjectDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            _kernel.Bind<IProductsRepository>().To<EfProductRepository>();
        }
    }
}