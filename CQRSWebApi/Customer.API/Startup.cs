using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Customer.Data;
using Customer.Data.IRepositories;
using Customer.Data.Repositories;
using Customer.Service.Dxos;
using System.Reflection;
using MediatR;
using Customer.Service.Services;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Customer.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CustomerDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //Add DIs
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerDxos, CustomerDxos>();

            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddLogging();
            services.AddControllers();


            //services.AddMediatR(typeof(Startup)); -> Sintaxis for the same assembly
            services.AddMediatR(typeof(CreateCustomerHandler).GetTypeInfo().Assembly); // -> Different assembly
                                                                                       //services.AddMediatorHandlers(typeof(CreateCustomerCommandExceptionHandler).GetTypeInfo().Assembly);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Customer.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Customer.API v1"));
            }

            app.UseExceptionHandler(builder =>
               builder.Run(async context =>
               {
                   context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                   var ex = context.Features.Get<IExceptionHandlerFeature>();
                   if (ex != null)
                   {
                       StringBuilder message = new StringBuilder();
                       if (env.IsDevelopment())
                       {
                           message.AppendLine(ex.Error.Message);
                           if (env.IsDevelopment())
                           {
                               message.AppendLine(ex.Error.StackTrace);
                           }
                       }
                       else
                       {
                           message.AppendLine("An error has occurred");
                       }

                       await context.Response.WriteAsync(message.ToString()).ConfigureAwait(false);
                   }
               }));

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
