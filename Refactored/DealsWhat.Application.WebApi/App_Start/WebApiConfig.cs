using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Autofac;
using Autofac.Integration.WebApi;
using DealsWhat.Application.WebApi.Controllers;
using DealsWhat.Application.WebApi.Models;
using DealsWhat.Domain.Interfaces;
using DealsWhat.Infrastructure.DataAccess;
using log4net;
using Microsoft.Owin.Security.OAuth;


namespace DealsWhat.Application.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance<IRepositoryFactory>(new EFRepositoryFactory(new DealsWhatUnitOfWork()));
            builder.RegisterApiControllers(typeof(FrontEndDealsController).Assembly);

            var container = builder.Build();

            using (var context = new DealsWhatUnitOfWork())
            {
                if (!context.Database.Exists())
                {
                    // Create the SimpleMembership database without Entity Framework migration schema
                    try
                    {
                        //((IObjectContextAdapter) context).ObjectContext.CreateDatabase();
                         SampleData.Seed(context);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }

            var resolver = new AutofacWebApiDependencyResolver(container);
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

        }


    }
}
