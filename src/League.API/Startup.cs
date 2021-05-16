using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Refit;
using League.API.Api;
using League.API.Models;
using League.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace League.API
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
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    //Could this go in a filter?
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var errorsInModelState = context.ModelState
                            .Where(x => x.Value.Errors.Count > 0)
                            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(x => x.ErrorMessage)).ToArray();

                        (string key, IEnumerable<string> value) = errorsInModelState.FirstOrDefault();

                        var subError = value.FirstOrDefault();

                        return new BadRequestObjectResult(ErrorResponse.GenerateErrorResponse(key, subError));
                    };
                });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "League.API", Version = "v1"});
            });
            services.AddTransient<IRiotSummonerService, RiotSummonerService>();
            services.AddRefitClient<IRiotApi>()
                .ConfigureHttpClient(client =>
                {
                    client.BaseAddress = new Uri(Configuration["RiotApi:BaseAddress"]);
                    client.DefaultRequestHeaders.Add("X-Riot-Token", Configuration["RiotApi:ApiKey"]);
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "League.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}