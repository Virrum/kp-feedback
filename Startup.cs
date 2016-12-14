// Copyright © 2014-present Kriasoft, LLC. All rights reserved.
// This source code is licensed under the MIT license found in the
// LICENSE.txt file in the root directory of this source tree.

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using SoundBoardPOC.Models;

namespace SoundBoardPOC
{
    public class Startup
    {
        // Load application settings from JSON file(s)
        // https://docs.asp.net/en/latest/fundamentals/configuration.html
        public Startup(IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile($"appsettings.json", optional: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .Build();
        }

        public IConfiguration Configuration { get; set; }

        // Configure IoC container
        // https://docs.asp.net/en/latest/fundamentals/dependency-injection.html
        public void ConfigureServices(IServiceCollection services)
        {
            // https://docs.asp.net/en/latest/security/anti-request-forgery.html
            services.AddAntiforgery(options => options.CookieName = options.HeaderName = "X-XSRF-TOKEN");

            // // Register Entity Framework database context
            // // https://docs.efproject.net/en/latest/platforms/aspnetcore/new-db.html
            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<DatabaseContext>()
                .AddDefaultTokenProviders();

            services.AddMvcCore()
                .AddAuthorization()
                .AddViews()
                .AddRazorViewEngine()
                .AddJsonFormatters();

            services.Configure<RazorViewEngineOptions>(options =>
                    {
                        options.ViewLocationExpanders.Clear();
                        options.ViewLocationExpanders.Add(new CustomViewLocationExpander());
                    });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory factory)
        {
            // Configure logging
            // https://docs.asp.net/en/latest/fundamentals/logging.html
            factory.AddConsole(Configuration.GetSection("Logging"));
            factory.AddDebug();
            var _webSocketCollection = new List<WebSocket>();

            app.UseDeveloperExceptionPage();
            // Serve static files
            // https://docs.asp.net/en/latest/fundamentals/static-files.html
            app.UseStaticFiles();

            // Enable external authentication provider(s)
            // https://docs.asp.net/en/latest/security/authentication/sociallogins.html
            app.UseIdentity();


            app.UseWebSockets();

            app.Use(async (context, next) =>
            {
                var http = (HttpContext)context;

                if (http.WebSockets.IsWebSocketRequest)
                {
                    var webSocket = await http.WebSockets.AcceptWebSocketAsync();
                    _webSocketCollection.Add(webSocket);
                    while (webSocket.State == WebSocketState.Open)
                    {
                        var token = CancellationToken.None;
                        var buffer = new ArraySegment<Byte>(new Byte[4096]);
                        var received = await webSocket.ReceiveAsync(buffer, token);
                        switch (received.MessageType)
                        {
                            case WebSocketMessageType.Text:
                                var request = Encoding.UTF8.GetString(buffer.Array,
                                                        buffer.Offset,
                                                        buffer.Count);
                                var type = WebSocketMessageType.Text;
                                var data = Encoding.UTF8.GetBytes(request);
                                buffer = new ArraySegment<Byte>(data);

                                _webSocketCollection.ForEach(async (socket) =>
                                {
                                    if (socket != null && socket.State == WebSocketState.Open)
                                    {
                                        await socket.SendAsync(buffer, type, true, token);
                                    }
                                });
                                // await webSocket.SendAsync(buffer, type, true, token);
                                break;
                        }
                    }
                }
                else
                {
                    await next();
                }
            });

            // Configure ASP.NET MVC
            // https://docs.asp.net/en/latest/mvc/index.html
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                     name: "default",
                     template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public static void Main()
        {
            var cwd = Directory.GetCurrentDirectory();
            var web = string.Empty;

            var host = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseWebRoot(web)
                .UseKestrel()
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
