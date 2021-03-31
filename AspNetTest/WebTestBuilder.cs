using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Security.Claims;

namespace AspNetTest
{
    public class WebTestBuilder
    {
        private readonly IServiceCollection Services = new ServiceCollection();
        private IConfiguration Configuration;

        public string UserName { get; private set; }
        public string[] UserRoles { get; private set; }

        public WebTestBuilder() { }

        public WebTest Build()
        {
            return new WebTest(Services);
        }

        public WebTestBuilder UseConfiguration(string userSecrets)
        {
            return UseConfiguration(options => options.AddUserSecrets(userSecrets));
        }

        public WebTestBuilder UseConfiguration(Action<ConfigurationBuilder> setup)
        {
            var builder = new ConfigurationBuilder();
            setup(builder);
            Configuration = builder.Build();
            return this;
        }

        public WebTestBuilder UseStartup<TStartup>() where TStartup : class
        {
            var startupType = typeof(TStartup);

            var configMethod = startupType.GetMethod("ConfigureServices");
            if (configMethod is null) throw new InvalidOperationException("Method not found.");

            var startup = Activator.CreateInstance(startupType, new object[] { Configuration });
            configMethod.Invoke(startup, new object[] { Services });

            return this;
        }

        public WebTestBuilder ConfigureServices(Action<IServiceCollection> configure)
        {
            configure(Services);
            return this;
        }

    }
}
