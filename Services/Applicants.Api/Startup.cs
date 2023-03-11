using System;
using Applicants.Api.Messaging.Consumers;
using MassTransit.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Applicants.Api.Services;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using MassTransit;


namespace Applicants.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IContainer ApplicationContainer { get; private set; }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddScoped<IApplicantRepository>(c => new ApplicantRepository(Configuration["ConnectionString"]));

            string rabbitmqHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST");              // ALAA added this 
            string rabbitmqUsername = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME");         // ALAA added this 
            string rabbitmqPassword = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD");          // ALAA added this 

            var builder = new ContainerBuilder();

            // register a specific consumer
            builder.RegisterType<ApplicantAppliedEventConsumer>();

            builder.Register(context =>
                {
                    var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
                    {
                        var host = cfg.Host(new Uri($"rabbitmq://{rabbitmqHost}/"), h =>    // ALAA added this 
                        {
                            h.Username(rabbitmqUsername); // ALAA added this 
                            h.Password(rabbitmqPassword);   // ALAA added this 
                        });

                        cfg.ReceiveEndpoint(host, "dotnetgigs" + Guid.NewGuid().ToString(), e =>
                        {
                            e.LoadFrom(context);
                        });
                    });

                    return busControl;
                })
                .SingleInstance()
                .As<IBusControl>()
                .As<IBus>();

            builder.Populate(services);
            ApplicationContainer = builder.Build();

            return new AutofacServiceProvider(ApplicationContainer);
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            var bus = ApplicationContainer.Resolve<IBusControl>();
            var busHandle = TaskUtil.Await(() => bus.StartAsync());
            lifetime.ApplicationStopping.Register(() => busHandle.Stop());
        }
    }
}
