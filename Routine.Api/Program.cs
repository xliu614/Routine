using Routine.Api.Data;
using Routine.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;

namespace Routine.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.
            //builder.Services.AddRazorPages();

            builder.Services.AddResponseCaching();
            builder.Services.AddControllers(
                setup =>
                {
                    setup.ReturnHttpNotAcceptable = true;
                    //setup to add a cache profile, usable at controller or action level
                    setup.CacheProfiles.Add("120sCachProfile", new CacheProfile
                    {
                        Duration = 120
                    });
                    //suppport XML output
                    //OutputFormatters is a collection, and by default it supports json, so one can add for XML, now we have both json and xml
                    //setup.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());

                    //if want to setup the default formater as for xml
                    //setup.OutputFormatters.Insert(0, new XmlDataContractSerializerOutputFormatter());
                })
                //the defaulted json serializer is replaced by NewtonsoftJson, so put this in front of the XML formatter
                .AddNewtonsoftJson(setup => {
                    setup.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                })
                .AddXmlDataContractSerializerFormatters() // this is to add xml formatter for both input and output formatters
                .ConfigureApiBehaviorOptions(setup => setup.InvalidModelStateResponseFactory = context =>
                  {
                      var websiteBaseUrl = $"{context.HttpContext.Request.Scheme}://{context.HttpContext.Request.Host}";
                      var problemDetails = new ValidationProblemDetails(context.ModelState) {
                          Type = websiteBaseUrl,
                          Title = "There is an error!",
                          Status = StatusCodes.Status422UnprocessableEntity,
                          Detail = "Please review the details",
                          Instance = context.HttpContext.Request.Path
                      };

                      problemDetails.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);

                      return new UnprocessableEntityObjectResult(problemDetails) {
                            ContentTypes = { "application/problem+json"}
                      };
                  })
                ;
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddTransient<ICompanyRepository, CompanyRepository>();
            builder.Services.AddTransient<IPropertyMappingService, PropertyMappingService>();
            builder.Services.AddTransient<IPropertyCheckerService, PropertyCheckerService>();
            builder.Services.AddDbContext<RoutineDbContext>(option =>
            {
                option.UseSqlite("Data Source=routine.db");
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();
            

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                try
                {
                    var dbContext = scope.ServiceProvider.GetService<RoutineDbContext>();
                    dbContext.Database.EnsureDeleted();
                    dbContext.Database.Migrate();
                }
                catch (Exception e)
                {
                    var logger = scope.ServiceProvider.GetService<ILogger<Program>>();
                    logger.LogError(e, "Database Migration Error");
                }
            }



            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("Unexpected Error on the server.");
                    });
                });
            }

            app.UseResponseCaching();
            app.UseRouting();

            app.UseAuthorization();
            //app.MapControllers();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapRazorPages(); //Routes for pages
                endpoints.MapControllers(); //Routes for my API controllers
            });



            app.Run();
        }
    }
}