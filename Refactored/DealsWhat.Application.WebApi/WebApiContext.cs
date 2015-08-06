using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dependencies;
using System.Web.Http.ExceptionHandling;
using Autofac;
using Autofac.Integration.WebApi;
using DealsWhat.Application.WebApi.Controllers;
using DealsWhat.Domain.Interfaces;
using DealsWhat.Domain.Services;
using DealsWhat.Infrastructure.DataAccess;
using log4net;

namespace DealsWhat.Application.WebApi
{
    public class WebApiContext
    {
        private static IDependencyResolver resolver;

        public static IDependencyResolver DefaultResolver
        {
            get
            {
                if (resolver == null)
                {
                    resolver = CreateResolver();
                }

                return resolver;
            }
            set { resolver = value; }
        }


        private static IDependencyResolver CreateResolver()
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance<IRepositoryFactory>(new EFRepositoryFactory(new DealsWhatUnitOfWork()));
            builder.RegisterApiControllers(typeof(FrontEndDealsController).Assembly);

            builder.RegisterType<DealService>().As<IDealService>();
            builder.RegisterType<CartService>().As<ICartService>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<OrderService>().As<IOrderService>();
            builder.RegisterType<EFUserRepository>().As<IUserRepository>();
          

            var container = builder.Build();

            var resolver = new AutofacWebApiDependencyResolver(container);

            return resolver;
        }

        internal class GlobalExceptionLogger : ExceptionLogger
        {
            private static readonly ILog Log4Net = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            public override void Log(ExceptionLoggerContext context)
            {
                Log4Net.Error(context.Exception.Message + context.Exception.StackTrace, context.Exception);
            }
        }
    }
}