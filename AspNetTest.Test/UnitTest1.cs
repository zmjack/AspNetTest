using AspNetTest.Test.Controllers;
using AspNetTest.Test.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AspNetTest.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var webTest = new WebTestBuilder().UseStartup<Startup>().Build();
            webTest.SetUser("123", new[] { "Admin" });

            var home = webTest.CreateController<HomeController>();
            var a = home.Index();
        }

        public class Startup
        {
            public Startup(IConfiguration configuration)
            {
                Configuration = configuration;
            }

            public IConfiguration Configuration { get; }

            // This method gets called by the runtime. Use this method to add services to the container.
            public void ConfigureServices(IServiceCollection services)
            {
                services.AddScoped(x => new NowService());
                services.AddScoped<INowService, NowService>(x => x.GetService<NowService>());
            }
        }

    }
}
