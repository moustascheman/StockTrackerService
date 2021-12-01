using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using StockTrackerService.Data;
using IBM.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StockTrackerService.Models;
using Confluent.Kafka.DependencyInjection;
using Confluent.Kafka;
using System.Net.Http;

namespace StockTrackerService
{
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
            //INITIALIZE DB HERE
            services.AddDbContext<StockTrackerContext>(opt => opt.UseDb2(@Configuration.GetConnectionString("dbstring"), p => p.SetServerInfo(IBMDBServerType.LUW)));
            services.AddScoped<IStockListingRepo, StockListingRepo>();
            services.AddLogging();
            services.AddHttpClient();
            services.AddSingleton<ProducerConfig>(opt => {
                ProducerConfig config = new ProducerConfig();
                config.BootstrapServers = Configuration["Kafka:default:BootstrapServers"];
                config.SaslUsername = Configuration["Kafka:default:SaslUsername"];
                config.SaslPassword = Configuration["Kafka:default:SaslPassword"];
                config.SecurityProtocol = SecurityProtocol.SaslSsl;
                config.SaslMechanism = SaslMechanism.Plain;
                config.SslEndpointIdentificationAlgorithm = SslEndpointIdentificationAlgorithm.Https;
                return config;
            });
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "StockTrackerService", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "StockTrackerService v1"));
            }

            IHttpClientFactory clientFactory = app.ApplicationServices.GetService(typeof(IHttpClientFactory)) as IHttpClientFactory;
            lifetime.ApplicationStarted.Register(RegisterToRegistry(clientFactory).Wait);
            lifetime.ApplicationStopped.Register(() => DeregisterService(clientFactory));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private async Task<Action> RegisterToRegistry(IHttpClientFactory _builder)
        {
            HttpClient client = _builder.CreateClient();
            string uri = Configuration["ServiceRegistry:uri"];
            uri = uri + "/" + "StockTracker" + "/" + "localhost" + "/" + "49301" + "/" + "StockTracker" + "/" + 1;
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, uri);
            try
            {
                HttpResponseMessage resp = await client.SendAsync(request);

            }
            catch
            {
                Console.WriteLine("Error when sending registration");
            }

            return null;
        }
        private Task<Action> DeregisterService(IHttpClientFactory factory)
        {
            HttpClient client = factory.CreateClient();
            string uri = Configuration["ServiceRegistry:uri"];
            uri = uri + "/" + "StockTracker";
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Delete, uri);
            try
            {
                HttpResponseMessage resp = client.Send(message);
                Console.WriteLine("Deregistered Service");
            }
            catch
            {
                Console.WriteLine("Error when deregistering service");
            }
            return null;
        }
    }

}
