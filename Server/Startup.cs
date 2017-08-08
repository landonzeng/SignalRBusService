using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Owin;
using System.Web.Http;

namespace ServerConsole
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //GlobalHost.DependencyResolver.UseRedis("127.0.0.1", 6379, string.Empty, "SignalRBus");
            //GlobalHost.DependencyResolver.UseSqlServer("Server=.;Initial Catalog=SignalRBus;User ID=sa;Password=123456");
            //允许CORS跨域
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
            app.UseWebApi(config);
        }
    }
}
