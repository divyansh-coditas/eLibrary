using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
using eLibrary.Services;

namespace eLibrary
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<BookDataAccess>();
            container.RegisterType<CategoryDataAccess>();
            container.RegisterType<SubscrptionDataAccess>();
            container.RegisterType<UserBookDataAccess>();
            container.RegisterType<UserDataAccess>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}