using SOTI.Project.DAL;
using SOTI.Project.DAL.Interfaces;
using System.Web.Http;
using Unity;
using Unity.WebApi;

namespace SOTI.Project.API
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            container.RegisterType<IProduct, ProductDetails>();//Disconnected
                                                               //container.RegisterType<IProduct, ProductDetailsConnect>();//Connected
            container.RegisterType<IProductAdditional, ProductDetails>();
            container.RegisterType<IAccount, UserDetails>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}
