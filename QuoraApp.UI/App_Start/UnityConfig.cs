using Unity;
using System.Web.Http;
using QuoraApp.ServiceLayer;
using System.Web.Mvc;

namespace QuoraApp.UI
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<IQuestionsService, QuestionsService>();
            container.RegisterType<IUsersService, UsersService>();
            container.RegisterType<ICategoriesService, CategoriesService>();
            container.RegisterType<IAnswersService, AnswersService>();

            //MVC5 Application Objects
            DependencyResolver.SetResolver(new Unity.Mvc5.UnityDependencyResolver(container));

            //Web API Object Deallocation
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
        }
    }
}