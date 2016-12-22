using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using MovieProjectWithUmbraco.Contexts;
using MovieProjectWithUmbraco.Repositories;
using MovieProjectWithUmbraco.Repositories.Interfaces;
using System.Data.Entity;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Umbraco.Web;

namespace MovieProjectWithUmbraco.DI_Configuration
{
    public class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            builder.Register(c => UmbracoContext.Current).AsSelf(); 
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(typeof(UmbracoApplication).Assembly);
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());


            builder.RegisterType<CustomDbContext>()
                .As<DbContext>();

            builder.RegisterType<FilmRatingRepository>()
                .As<IFilmRatingRepository>();

            var container = builder.Build();
            var resolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = resolver;
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}