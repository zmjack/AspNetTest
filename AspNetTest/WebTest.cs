using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Security.Claims;

namespace AspNetTest
{
    public class WebTest
    {
        private readonly IServiceCollection Services;
        public string UserName { get; set; }
        public string[] UserRoles { get; set; }

        public WebTest(IServiceCollection services)
        {
            Services = services;
        }

        public void SetUser(string name)
        {
            UserName = name;
            UserRoles = new string[0];
        }

        public void SetUser(string name, string[] roles)
        {
            UserName = name;
            UserRoles = roles;
        }

        public ServiceProvider BuildServiceProvider() => Services.BuildServiceProvider();

        public TController CreateController<TController>() where TController : ControllerBase
        {
            var requestServices = BuildServiceProvider();
            var constructor = typeof(TController).GetConstructors().First();
            var arguments = constructor.GetParameters().Select(x => requestServices.GetService(x.ParameterType)).ToArray();
            var controller = constructor.Invoke(arguments) as TController;

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(ClaimsIdentityEx.Create(UserName, UserRoles)),
                },
            };
            controller.HttpContext.RequestServices = requestServices;

            return controller;
        }

    }
}
